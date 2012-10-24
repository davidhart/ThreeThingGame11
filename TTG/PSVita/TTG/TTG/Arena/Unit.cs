using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TTG
{
    public class AttackHandler
    {
        public virtual void OnAttack(Unit unit, Unit target)
        {
        }
    }

    public class DeathHandler
    {
        public virtual void OnDeath(Unit unit)
        {
        }
    }

    public class BulletAttackHandler : AttackHandler
    {
        Arena _arena;
        Color _color1;
        Color _color2;

        public BulletAttackHandler(Arena arena, Color color1, Color color2)
        {
            _arena = arena;
            _color1 = color1;
            _color2 = color2;
        }

        public override void OnAttack(Unit unit, Unit target)
        {
            _arena.AddMarineShot(unit, target, _color1, _color2); 
        }
    }

    public class ProjectileAttackHandler : AttackHandler
    {
        Arena _arena;
        Texture2D _texture;

        public ProjectileAttackHandler(Arena arena, Texture2D texture)
        {
            _arena = arena;
            _texture = texture;
        }

        public override void OnAttack(Unit unit, Unit target)
        {
            _arena.AddProjectile(new Projectile(unit, target, _texture));
        }
    }

    public class JuggernaughtDeathHandler : DeathHandler
    {
        Arena _arena;

        public JuggernaughtDeathHandler(Arena arena)
        {
            _arena = arena;
        }
        
        public override void OnDeath(Unit unit)
        {
            Unit u = _arena.AddUnit(UnitEnum.Marine, unit.GetTeam());
            u.SetPosition(unit.GetPosition());
        }
    }

    public class UnitProperties
    {
        // Unit animations
        public Animation _move;
        public Animation _attack;

        // Amount of damage per attack
        public int _attackDamage;

        // Total hitpoints
        public int _maxHp;

        // Attack frequency (number of attacks per second)
        public float _attackSpeed;

        // Range to start attacking an enemy
        public float _attackRange;

        // Range to start following an enemy
        public float _followRange;

        // Speed to move towards target in pixels per second
        public float _moveSpeed;

        // What type of unit is this (flying / ground)
        public UnitType _type;

        // What type of unit can be targetted
        public TargetUnitType _targetType;

        // Handler for specific behaviours for this type of unit
        public AttackHandler _attackHandler;
        public DeathHandler _deathHandler;

        public bool _isLarge;

        public UnitProperties()
        {
            _attackHandler = new AttackHandler();
            _deathHandler = new DeathHandler();

            _isLarge = false;
        }
    }

    public class Unit
    {
        private const int FLY_OFFSET = 60;

        private UnitProperties _properties;

        private Arena _arena;

        // Time remaining to next attack
        private float _nextAttack;

        // Current target
        private Unit _target;

        // Elapsed time since unit creation (for bobbing animation of flying units)
        private float _elapsed;

        private AnimationPlayer _animationPlayer;

        private int _hitPoints;

        private UnitTeam _team;

        private Vector2 _position;

        private float _hitCooldown;
        private const float _hitDuration = 0.1f;

        public Unit(UnitProperties properties, Vector2 position, UnitTeam team, Arena arena)
        {
            _team = team;
            _arena = arena;
            _properties = properties;
            _position = position;

            _hitPoints = properties._maxHp;

            _elapsed = 0;

            _animationPlayer = new AnimationPlayer();
            _animationPlayer.PlayAnimation(_properties._move);

            _target = null;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _hitCooldown = Math.Max(0, _hitCooldown - dt);

            _nextAttack -= dt;
            _elapsed += dt;

            bool attacked = false;

            // If this unit can shoot
            if (_properties._targetType != TargetUnitType.None)
            {
                // If we don't have a target try get a new one
                if (_target == null || _target.IsDead())
                {
                    _target = _arena.AcquireTarget(this);
                }

                if (_target != null)
                {
                    Vector2 direction = _target.GetMidPoint() - GetMidPoint();
                    float distance = direction.Length();

                    // Attack if target is in attack range
                    if (distance < _properties._attackRange)
                    {
                        while (_nextAttack <= 0)
                        {
                            _nextAttack += _properties._attackSpeed;
                            OnAttack(_target);
                        }

                        attacked = true;
                    }
                    else
                    {
                        // If target is now outside follow range we lost it (rarely if ever happens)
                        if (distance > _properties._followRange)
                        {
                            _target = null;
                        }
                        // Otherwise we have a target in follow range but not attack range so move towards it
                        else
                        {
                            direction.Normalize();
                            _position += direction * _properties._moveSpeed * dt;
                            _animationPlayer.PlayAnimation(_properties._move);
                        }
                    }
                }
            }
            
            // If we have no target default to walking in the spawn direction
            if (_target == null)
            {
                if (_team == UnitTeam.Player1)
                    _position.X += dt * _properties._moveSpeed;
                else
                    _position.X -= dt * _properties._moveSpeed;

                _animationPlayer.PlayAnimation(_properties._move);
            }

            // Prevent "banking" attacks while following a target
            if (!attacked)
            {
                _nextAttack = Math.Max(0, _nextAttack);
            }

            _animationPlayer.Update(gameTime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            SpriteEffects flip = SpriteEffects.None;

            if (_team == UnitTeam.Player2)
            {
                flip = SpriteEffects.FlipHorizontally;
            }

            if (_target != null && !_target.IsDead())
            {
                float targetDir = _target.GetPosition().X - _position.X;

                if (targetDir < 0)
                    flip = SpriteEffects.FlipHorizontally;
                else
                    flip = SpriteEffects.None;
            }

            Vector2 pos = GetDrawPosition();

            spritebatch.Draw(_animationPlayer.GetCurrentFrameTexture(), new Vector2((float)Math.Floor(pos.X), (float)Math.Floor(pos.Y)), 
                _animationPlayer.GetCurrentFrameRectangle(), GetHitColor(), 0, Vector2.Zero, 1, flip, _position.Y / 600);
        }

        public void DrawShadow(SpriteBatch spritebatch, Texture2D spriteSheet)
        {
            Vector2 offset = new Vector2(0, 0);

            if (_properties._isLarge)
            {
                Rectangle largeRectangle = new Rectangle(16, 96, 32, 16);
                spritebatch.Draw(spriteSheet, _position + offset, largeRectangle, Color.White);
            }
            else
            {
                Rectangle smallRectangle = new Rectangle(0, 96, 16, 16);
                spritebatch.Draw(spriteSheet, _position + offset, smallRectangle, Color.White);
            }
        }

        public bool CanTarget(Unit target)
        {
            if (target.GetTeam() == _team)
                return false;

            if (target.GetUnitType() == UnitType.Air && _properties._targetType == TargetUnitType.GroundOnly)
                return false;

            if (target.GetUnitType() == UnitType.Ground && _properties._targetType == TargetUnitType.AirOnly)
                return false;

            return true;
        }

        public Rectangle GetRect()
        {
            Rectangle r = _properties._move.GetFrameRect(0);
            return new Rectangle(0, 0, r.Width, r.Height);
        }

        public Vector2 GetDrawPosition()
        {
            Vector2 pos = _position;

            if (_properties._type == UnitType.Air)
            {
                float offset = (float)Math.Sin(_elapsed * 5.0f);

                float amplitude = 5.0f;

                if (_target != null)
                    amplitude = 2.0f;

                pos -= new Vector2(0, FLY_OFFSET + offset * amplitude);
            }

            return pos;
        }

        private void OnAttack(Unit target)
        {
            _animationPlayer.PlayAnimation(_properties._attack);
            _animationPlayer.ResetAnimation();

            _properties._attackHandler.OnAttack(this, target);
        }

        public Vector2 GetMidPoint()
        {
            Rectangle r = _properties._move.GetFrameRect(0);
            return _position + new Vector2(r.Width, r.Height);
        }

        public void OnDeath(PEmitter de)
        {
            de.Active = true;
            de.RecycleParticles();
            de.pos = GetMidPoint();

            _properties._deathHandler.OnDeath(this);
        }

        public int AttackDamage()
        {
            return _properties._attackDamage;
        }

        public float FollowRange()
        {
            return _properties._followRange;
        }

        public void TakeDamage(int damage)
        {
            _hitPoints = Math.Max(0, _hitPoints - damage);
            _hitCooldown = _hitDuration;
        }

        public void Kill()
        {
            _hitPoints = 0;
        }

        public bool IsDead()
        {
            return _hitPoints <= 0;
        }

        public Color GetHitColor()
        {
            return Color.Lerp(Color.White, Color.IndianRed, _hitCooldown / _hitDuration);
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public UnitTeam GetTeam()
        {
            return _team;
        }

        public int GetHp()
        {
            return _hitPoints;
        }

        public int GetMaxHp()
        {
            return _properties._maxHp;
        }

        public UnitType GetUnitType()
        {
            return _properties._type;
        }
    }
}
