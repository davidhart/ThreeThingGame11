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
            Unit u = _arena.AddUnit(UnitEnum.Marine, unit.Team);
            u.Position = unit.Position;
        }
    }

    public class UnitProperties
    {
        // Unit animations
        private Animation _move;
        public Animation MoveAnim
        {
            get
            {
                return _move;
            }
            set
            {
                _move = value;
            }
        }
        private Animation _attack;
        public Animation AtkAnim
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
            }
        }

        // Amount of damage per attack
        private int _attackDamage;
        public int AttackDamage
        {
            get
            {
                return _attackDamage;
            }
            set
            {
                _attackDamage = value;
            }
        }

        // Total hitpoints
        private int _maxHp;
        public int MaxHp
        {
            get
            {
                return _maxHp;
            }
            set
            {
                _maxHp = value;
            }
        }

        // Attack frequency (number of attacks per second)
        private float _attackSpeed;
        public float AttackSpeed
        {
            get
            {
                return _attackSpeed;
            }
            set
            {
                _attackSpeed = value;
            }
        }

        // Range to start attacking an enemy
        public float _attackRange;
        public float AttackRange
        {
            get
            {
                return _attackRange;
            }
            set
            {
                _attackRange = value;
            }
        }

        // Range to start following an enemy
        private float _followRange;
        public float FollowRange
        {
            get
            {
                return _followRange;
            }
            set
            {
                _followRange = value;
            }
        }

        // Speed to move towards target in pixels per second
        private float _moveSpeed;
        public float MoveSpeed
        {
            get
            {
                return _moveSpeed;
            }
            set
            {
                _moveSpeed = value;
            }
        }

        // What type of unit is this (flying / ground)
        private UnitType _type;
        public UnitType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        // What type of unit can be targetted
        private TargetUnitType _targetType;
        public TargetUnitType TargetType
        {
            get
            {
                return _targetType;
            }
            set
            {
                _targetType = value;
            }
        }

        // Handler for specific behaviours for this type of unit
        private AttackHandler _attackHandler;
        public AttackHandler AttackHandler
        {
            get
            {
                return _attackHandler;
            }
            set
            {
                _attackHandler = value;
            }
        }
        private DeathHandler _deathHandler;
        public DeathHandler DeathHandler
        {
            get
            {
                return _deathHandler;
            }
            set
            {
                _deathHandler = value;
            }
        }

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
        private readonly int FLY_OFFSET = 60;

        private UnitProperties _properties;

        private Arena _arena;

        // Time remaining to next attack
        private float _nextAttack;
        public float NextAttack
        {
            get
            {
                return _nextAttack;
            }
            set
            {
                _nextAttack = value;
            }
        }

        // Current target
        private Unit _target;
        public Unit Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        // Elapsed time since unit creation (for bobbing animation of flying units)
        private float _elapsed;
        public float Elapsed
        {
            get
            {
                return _elapsed;
            }
            set
            {
                _elapsed = value;
            }
        }

        private AnimationPlayer _animationPlayer;

        private int _hitPoints;
        public int HitPoints
        {
            get
            {
                return _hitPoints;
            }
            set
            {
                _hitPoints = value;
            }
        }

        private UnitTeam _team;
        public UnitTeam Team
        {
            get
            {
                return _team;
            }
            set
            {
                _team = value;
            }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        private float _hitCooldown;
        private readonly float _hitDuration = 0.1f;

        public Unit(UnitProperties properties, Vector2 position, UnitTeam team, Arena arena)
        {
            _team = team;
            _arena = arena;
            _properties = properties;
            _position = position;

            _hitPoints = properties.MaxHp;

            _elapsed = 0;

            _animationPlayer = new AnimationPlayer();
            _animationPlayer.PlayAnimation(_properties.MoveAnim);

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
            if (_properties.TargetType != TargetUnitType.None)
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
                            _nextAttack += _properties.AttackSpeed;
                            OnAttack(_target);
                        }

                        attacked = true;
                    }
                    else
                    {
                        // If target is now outside follow range we lost it (rarely if ever happens)
                        if (distance > _properties.FollowRange)
                        {
                            _target = null;
                        }
                        // Otherwise we have a target in follow range but not attack range so move towards it
                        else
                        {
                            direction.Normalize();
                            _position += direction * _properties.MoveSpeed * dt;
                            _animationPlayer.PlayAnimation(_properties.MoveAnim);
                        }
                    }
                }
            }
            
            // If we have no target default to walking in the spawn direction
            if (_target == null)
            {
                if (_team == UnitTeam.Player1)
                    _position.X += dt * _properties.MoveSpeed;
                else
                    _position.X -= dt * _properties.MoveSpeed;

                _animationPlayer.PlayAnimation(_properties.MoveAnim);
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
            if (target.Team == _team)
                return false;

            if (target.GetUnitType() == UnitType.Air && _properties.TargetType == TargetUnitType.GroundOnly)
                return false;

            if (target.GetUnitType() == UnitType.Ground && _properties.TargetType == TargetUnitType.AirOnly)
                return false;

            return true;
        }

        public Rectangle GetRect()
        {
            Rectangle r = _properties.MoveAnim.GetFrameRect(0);
            return new Rectangle(0, 0, r.Width, r.Height);
        }

        public Vector2 GetDrawPosition()
        {
            Vector2 pos = _position;

            if (_properties.Type == UnitType.Air)
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
            _animationPlayer.PlayAnimation(_properties.AtkAnim);
            _animationPlayer.ResetAnimation();

            _properties.AttackHandler.OnAttack(this, target);
        }

        public Vector2 GetMidPoint()
        {
            Rectangle r = _properties.MoveAnim.GetFrameRect(0);
            return _position + new Vector2(r.Width, r.Height);
        }

        public void OnDeath(PEmitter de)
        {
            de.Active = true;
            de.RecycleParticles();
            de.pos = GetMidPoint();

            _properties.DeathHandler.OnDeath(this);
        }

        public int AttackDamage()
        {
            return _properties.AttackDamage;
        }

        public float FollowRange()
        {
            return _properties.FollowRange;
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

        public int GetMaxHp()
        {
            return _properties.MaxHp;
        }

        public UnitType GetUnitType()
        {
            return _properties.Type;
        }
    }
}
