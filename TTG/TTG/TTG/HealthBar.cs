using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TTG
{
    class HealthBar
    {
        private Target _target;
        private Vector2 _position;
        private int _width;
        private Texture2D _barBGLeft, _barBGRight, _barBG, _bar;

        public HealthBar(Target target, Vector2 position, int width)
        {
            _position = position;
            _target = target;
            _width = width;
        }

        public void LoadContent(ContentManager content)
        {
            _barBGLeft = content.Load<Texture2D>("hpBarEndL");
            _barBGRight = content.Load<Texture2D>("hpBarEndR");
            _barBG = content.Load<Texture2D>("hpBarBg");
            _bar = content.Load<Texture2D>("hpBar");
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_barBGLeft, _position, Color.White);
            spritebatch.Draw(_barBG, new Rectangle((int)_position.X + _barBGLeft.Width, (int)_position.Y, _width - (int)_position.X - _barBGLeft.Width - _barBGRight.Width, _barBG.Height), Color.White);
            spritebatch.Draw(_barBGRight, new Vector2(_position.X + _width - _barBGLeft.Width, _barBGRight.Width), Color.White);

            int barWidth = _width - _barBGLeft.Width - _barBGRight.Width;

            barWidth = (int)(barWidth * (float)_target.HP / _target.MaxHP);

            spritebatch.Draw(_bar, new Rectangle((int)_position.X + _barBGLeft.Width, (int)_position.Y, barWidth, _bar.Height), Color.White);
        }


    }
}
