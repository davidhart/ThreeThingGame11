using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class Base : Target
    {
        private Texture2D _texture;
        public Base(Vector2 position, UnitTeam team, Texture2D texture) : 
            base(position, team)
        {
            _texture = texture;
            MaxHP = 10000;
        }

        public override Rectangle GetRect()
        {
            return new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, _position, GetHitColor());
        }

        public override Vector2 GetMidPoint()
        {
            return _position + new Vector2(_texture.Width / 2, _texture.Height / 2);
        }
    }
}
