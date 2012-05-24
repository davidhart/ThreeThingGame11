using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TTG
{
    public class AttackHandler
    {
        public virtual void OnAttack(Unit unit, Target target)
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

        public override void OnAttack(Unit unit, Target target)
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

        public override void OnAttack(Unit unit, Target target)
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
            Unit u = _arena.AddUnit(UnitEnum.Marine, unit.Team);
            u.Position = unit.Position;
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

        public UnitProperties()
        {
            _attackHandler = new AttackHandler();
            _deathHandler = new DeathHandler();
        }
    }

    public class Unit : Target
    {
        private UnitProperties _properties;

        private Arena _arena;

        // Time remaining to next attack
        private float _nextAttack;

        // Current target
        private Target _target;

        // Elapsed time since unit creation (for bobbing animation of flying units)
        private float _elapsed;

        private AnimationPlayer _animationPlayer;

        public Unit(UnitProperties properties, Vector2 position, UnitTeam team, Arena arena)
            : base(position, team)
        {
            SetMaxHp(properties._maxHp);
            SetHp(properties._maxHp);

            _type = properties._type; // TODO: remove this member variable?

            _properties = properties;
            _elapsed = 0;
            _arena = arena;

            _animationPlayer = new AnimationPlayer();
            _animationPlayer.PlayAnimation(_properties._move);

            _target = null;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _nextAttack -= dt;
            _elapsed += dt;

            bool attacked = false;

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
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            SpriteEffects flip = SpriteEffects.None;

            if (_team == UnitTeam.Player2)
            {
                flip = SpriteEffects.FlipHorizontally;
            }

            if (_target != null && !_target.IsDead())
            {
                float targetDir = _target.Position.X - _position.X;

                if (targetDir < 0)
                    flip = SpriteEffects.FlipHorizontally;
                else
                    flip = SpriteEffects.None;
            }

            Vector2 pos = GetDrawPosition();

            spritebatch.Draw(_animationPlayer.GetCurrentFrameTexture(), new Vector2((float)Math.Floor(pos.X), (float)Math.Floor(pos.Y)), 
                _animationPlayer.GetCurrentFrameRectangle(), GetHitColor(), 0, Vector2.Zero, 1, flip, _position.Y / 600);
        }

        public bool CanTarget(Target target)
        {
            if (target.Team == _team)
                return false;

            if (target.Type == UnitType.Air && _properties._targetType == TargetUnitType.GroundOnly)
                return false;

            if (target.Type == UnitType.Ground && _properties._targetType == TargetUnitType.AirOnly)
                return false;

            return true;
        }

        public override Rectangle GetRect()
        {
            Rectangle r = _properties._move.GetFrameRect(0);
            return new Rectangle(0, 0, r.Width, r.Height);
        }

        public override Vector2 GetDrawPosition()
        {
            Vector2 pos = _position;

            if (_type == UnitType.Air)
            {
                float offset = (float)Math.Sin(_elapsed * 5.0f);

                float amplitude = 5.0f;

                if (_target != null)
                    amplitude = 2.0f;

                pos -= new Vector2(0, 40 + offset * amplitude);
            }

            return pos;
        }

        protected virtual void OnAttack(Target target)
        {
            _animationPlayer.PlayAnimation(_properties._attack);
            _animationPlayer.ResetAnimation();

            _properties._attackHandler.OnAttack(this, target);
        }

        public override Vector2 GetMidPoint()
        {
            Rectangle r = _properties._move.GetFrameRect(0);
            return _position + new Vector2(r.Width, r.Height);
        }

        public override void OnDeath(PEmitter de)
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
    }
}
