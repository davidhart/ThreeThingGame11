using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TTG
{
    public abstract class Unit : Target
    {
        protected int _attackDamage;
        public int AttackDamage
        {
            get
            {
                return _attackDamage;
            }
        }

        protected float _attackRange;
        protected float _followRange;

        public float FollowRange
        {
            get 
            { 
                return _followRange; 
            }
        }

        protected float _attackSpeed;
        protected float _moveSpeed;
        private Target _target;

        protected TargetUnitType _targetType;

        private AnimationPlayer _animationPlayer;
        
        protected Arena _arena;
        protected Animation _animationMove;
        protected Animation _animationAttack;

        private float _nextAttack;

        // Particle System Stuff!
        protected MarineDeathEmitter ps;
        protected Vector2 psPosition;

        public Unit(Vector2 position, UnitTeam team, Arena arena, Animation animationMove, Animation animationAttack)
            : base(position, team)
        {
            _targetType = TargetUnitType.Any;
            _type = UnitType.Ground;
            _arena = arena;

            _animationPlayer = new AnimationPlayer();
            _animationPlayer.PlayAnimation(animationMove);

            _animationMove = animationMove;
            _animationAttack = animationAttack;

            _target = null;
        }

        public override void Update(GameTime gameTime)
        {
            // attack
            _nextAttack -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_target == null || _target.IsDead())
            {
                _target = _arena.AcquireTarget(this);
            }

            if (_target != null)
            {
                Vector2 direction = _target.GetMidPoint() - GetMidPoint();
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
                       //_target.TakeDamage(_attackDamage);
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

            spritebatch.Draw(_animationPlayer.GetCurrentFrameTexture(), new Vector2((float)Math.Floor(_position.X), (float)Math.Floor(_position.Y)), 
                _animationPlayer.GetCurrentFrameRectangle(), GetHitColor(), 0, Vector2.Zero, 1, flip, _position.Y / 600);
        }

        public bool CanTarget(Target target)
        {
            if (target.Team == _team)
                return false;

            if (target.Type == UnitType.Air && _targetType == TargetUnitType.GroundOnly)
                return false;

            if (target.Type == UnitType.Ground && _targetType == TargetUnitType.AirOnly)
                return false;

            return true;
        }

        public override Rectangle GetRect()
        {
            Rectangle r = _animationMove.GetFrameRect(0);
            return new Rectangle(0, 0, r.Width, r.Height);
        }

        protected virtual void OnAttack(Target target)
        {
            _animationPlayer.PlayAnimation(_animationAttack);
            _animationPlayer.ResetAnimation();
        }

        public override Vector2 GetMidPoint()
        {
            Rectangle r = _animationMove.GetFrameRect(0);
            return _position + new Vector2(r.Width, r.Height);
        }

        public override void OnDeath(PEmitter de)
        {
            Rectangle r = _animationMove.GetFrameRect(0);
            de.Active = true;
            de.pos = _position + new Vector2(r.Width, r.Height);
        }
    }
}
