using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class Projectile
    {
        protected float _lifetime = 0.8f;
        private float _elapsed;

        private Vector2 _start;
        private Vector2 _end;
        private Vector2 _direction;
        private Unit _target;
        private Unit _attacker;
        private Texture2D _texture;

        public Projectile(Unit attacker, Unit target, Texture2D texture)
        {
            _texture = texture;
            _start = attacker.GetDrawPosition() - new Vector2(texture.Width / 2, texture.Height / 2);
            Rectangle r = target.GetRect();
            _end = target.GetDrawPosition() + Util.RandVector(r.Width, r.Height); // TODO: random target point
            
            _direction = _end - _start;

            _target = target;
            _attacker = attacker;
            _elapsed = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsed > _lifetime)
                _target.TakeDamage(_attacker.AttackDamage());
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, GetPosition(), Color.White);
        }

        public float PercentAlive()
        {
            return _elapsed / _lifetime;
        }

        public Vector2 GetPosition()
        {
            return _start + _direction * PercentAlive();
        }

        public bool IsDead()
        {
            return _elapsed > _lifetime;
        }
    }


    public class ProjectileBatch
    {
        List<Projectile> _projectiles;

        public ProjectileBatch()
        {
            _projectiles = new List<Projectile>();
        }

        public void AddProjectile(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _projectiles.Count; )
            {
                _projectiles[i].Update(gameTime);

                if (_projectiles[i].IsDead())
                {
                    _projectiles.RemoveAt(i);
                }
                else
                    ++i;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Draw(spritebatch);
            }
        }

        public void Clear()
        {
            _projectiles.Clear();
        }
    }
}
