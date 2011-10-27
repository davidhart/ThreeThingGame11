using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TTG
{
    public class Unit
    {
        protected int _attackDamage;
        protected float _attackRange;
        protected float _followRange;

        public float FollowRange
        {
            get { return _followRange; }
        }

        protected float _attackSpeed;
        protected float _moveSpeed;
        protected int _hitPoints;
        private Unit _target;

        protected TargetUnitType _targetType;
        protected UnitType _type;
        private UnitTeam _team;

        protected Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
        }

        protected Arena _arena;

        private AnimationPlayer _animationPlayer;

        protected Animation _animationMove;
        protected Animation _animationAttack;

        private float _nextAttack;

        public Unit(Vector2 position, UnitTeam team, Arena arena, Animation animationMove, Animation animationAttack)
        {
            _targetType = TargetUnitType.Any;
            _type = UnitType.Ground;
            _team = team;

            _animationPlayer = new AnimationPlayer();
            _position = position;

            _animationPlayer.PlayAnimation(animationMove);

            _animationMove = animationMove;
            _animationAttack = animationAttack;

            _arena = arena;

            _target = null;
        }

        public virtual void Update(GameTime gameTime)
        {
            // attack
            _nextAttack -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_target == null || _target.IsDead())
            {
                _target = _arena.AcquireTarget(this);
            }

            if (_target != null)
            {
                Vector2 direction = _target._position - _position;
                float distance = direction.Length();

                if (distance > _followRange)
                {
                    // lost target
                    _target = null;
                }
                else if (distance < _attackRange)
                {
                    // attack
                    while (_nextAttack <= 0)
                    {
                        _nextAttack += _attackSpeed;
                        _target.TakeDamage(_attackDamage);
                        OnAttack(_target);
                    }
                }
                else
                {
                    // follow
                    direction.Normalize();
                    _position += direction * _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _animationPlayer.PlayAnimation(_animationMove);
                }
            }
            
            if (_target == null)
            {
                _nextAttack = Math.Max(0, _nextAttack);

                if (_team == UnitTeam.Player1)
                    _position.X += (float)gameTime.ElapsedGameTime.TotalSeconds * _moveSpeed;
                else
                    _position.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * _moveSpeed;

                _animationPlayer.PlayAnimation(_animationMove);
            }

            _animationPlayer.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            SpriteEffects flip = SpriteEffects.None;

            if (_team == UnitTeam.Player2)
            {
                flip = SpriteEffects.FlipHorizontally;
            }

            if (_target != null && !_target.IsDead())
            {
                float targetDir = _target._position.X - _position.X;

                if (targetDir < 0)
                    flip = SpriteEffects.FlipHorizontally;
                else
                    flip = SpriteEffects.None;
            }

            _animationPlayer.Draw(spritebatch, new Vector2((float)Math.Floor(_position.X), (float)Math.Floor(_position.Y)), flip);
        }

        public void TakeDamage(int damage)
        {
            _hitPoints = Math.Max(0, _hitPoints - damage);
        }

        public void Kill()
        {
            _hitPoints = 0;
        }

        public bool IsDead()
        {
            return _hitPoints <= 0;
        }

        public bool CanTarget(Unit target)
        {
            if (target._team == _team)
                return false;

            if (target._type == UnitType.Air && _targetType == TargetUnitType.GroundOnly)
                return false;

            if (target._type == UnitType.Ground && _targetType == TargetUnitType.AirOnly)
                return false;

            return true;
        }

        public virtual Rectangle GetRect()
        {
            Rectangle r = _animationMove.GetFrameRect(0);
            return new Rectangle(0, 0, r.Width, r.Height);
        }

        protected virtual void OnAttack(Unit target)
        {
            _animationPlayer.PlayAnimation(_animationAttack);
        }
    }
}
