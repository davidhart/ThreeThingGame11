using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    class Block
    {
        int _id;

        int _x;
        int _y;

        Texture2D _texture;

        public Block(int id, Texture2D texture, int xPos, int yPos)
        {
            _id = id;

            _x = xPos;
            _y = yPos;

            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_texture, new Rectangle(_x, _y, 64, 64), Color.White);

            spriteBatch.End();
        }

        public int GetID()
        {
            return _id;
        }
    }
}
