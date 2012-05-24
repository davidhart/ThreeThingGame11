using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TTG
{
    public class UnitProperties
    {
        public Animation _move;
        public Animation _attack;

        public int _attackDamage;
        public int _attackRange;
        public int _followRange;

    }

    public abstract class Unit : Target
    {
        protected int _attackDamage;

        protected float _attackRange;
        protected float _followRange;

        protected float _attackSpeed;
        protected float _moveSpeed;
        private Target _target;

        protected TargetUnitType _targetType;

        private AnimationPlayer _animationPlayer;
        
        protected Arena _arena;
        protected Animation _animationMove;
        protected Animation _animationAttack;

        private float _nextAttack;

        protected MarineDeathEmitter ps;
        protected Vector2 psPosition;

        private float _elapsed;

        public Unit(Vector2 position, UnitTeam team, Arena arena, Animation animationMove, Animation animationAttack)
            : base(position, team)
        {
            _elapsed = 0;
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
            _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_target == null || _target.IsDead())
            {
                _target = _arena.AcquireTarget(this);
            }

            if (_target != null)
            {
                Vector2 direction = _target.GetMidPoint() - GetMidPoint();
                float distance = direction.Length();

                if (distance < _attackRange)
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
                    _nextAttack = 0;
                    if (distance > _followRange)
                    {
                        // lost target
                        _target = null;
                    }
                    else
                    {
                        // follow
                        direction.Normalize();
                        _position += direction * _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _animationPlayer.PlayAnimation(_animationMove);
                    }
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

            Vector2 pos = GetDrawPosition();

            spritebatch.Draw(_animationPlayer.GetCurrentFrameTexture(), new Vector2((float)Math.Floor(pos.X), (float)Math.Floor(pos.Y)), 
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

        public override Vector2  GetDrawPosition()
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
            de.Active = true;
            de.RecycleParticles();
            de.pos = GetMidPoint();
        }

        public int AttackDamage()
        {
            return _attackDamage;
        }

        public float FollowRange()
        {
            return _followRange;
        }
    }
}
