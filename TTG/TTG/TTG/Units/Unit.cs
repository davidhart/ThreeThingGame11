using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class Unit
    {
        protected float _attackDamage;
        protected float _attackRange;
        protected float _attackSpeed;
        protected float _moveSpeed;
        protected int _hitPoints;
        Unit _target;

        private AnimationPlayer _animationPlayer;
        
        // TODO: Add Sound
        // protected SoundEffect spawnSound, dieSound;

        protected Vector2 _position;

        public Unit(Vector2 position)
        {
            _animationPlayer = new AnimationPlayer();
            _position = position;

            _target = null;
        }

        public virtual void Update(GameTime gameTime)
        {
            _animationPlayer.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            _animationPlayer.Draw(spritebatch, _position, SpriteEffects.None);
        }

        public void PlayAnimation(Animation animation)
        {
            _animationPlayer.PlayAnimation(animation);
        }
    }
}
