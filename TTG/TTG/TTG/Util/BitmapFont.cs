using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TTG
{
    class BitmapFont
    {
        string _name;
        Texture2D _texture;

        int _charactersX;
        int _charactersY;
        int _spacing;
        int _characterWidth;
        int _characterHeight;
        int _scale;

        public BitmapFont(string fontName, int charactersX, int charactersY)
        {
            _name = fontName;
            _charactersX = charactersX;
            _charactersY = charactersY;
            _spacing = 0;
            _characterWidth = 0;
            _characterHeight = 0;
            _scale = 1;
        }

        public void SetSpacing(int spacing)
        {
            _spacing = spacing;
        }

        public void SetScale(int scale)
        {
            _scale = scale;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("fonts/" + _name);
            _characterWidth = _texture.Width / _charactersX;
            _characterHeight = _texture.Height / _charactersY;
        }

        public void DrawText(SpriteBatch batch, string text, Vector2 position)
        {
            DrawText(batch, text, position, Color.White);
        }

        private void GetSourceRectFromIndex(int index, ref Rectangle r)
        {
            r = new Rectangle((index % _charactersX) * _characterWidth, 
                              (index / _charactersX) * _characterHeight, 
                              _characterWidth, 
                              _characterHeight);
        }

        private bool GetCharacterSourceRect(char character, ref Rectangle r)
        {
            int charIndex = -1;

            if (character == 'x')
                charIndex = 36;
            if (character >= 'A' && character <= 'Z')
                charIndex = character - 'A';
            else if (character >= '0' && character <= '9')
                charIndex = character - '0' + 26;

            if (charIndex <= 0)
                return false;

            GetSourceRectFromIndex(charIndex, ref r);

            return true;
        }

        public void DrawText(SpriteBatch batch, string text, Vector2 position, Color color)
        {
            Rectangle sourceRect = new Rectangle();
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, _characterWidth * _scale, _characterHeight * _scale);
            bool draw = false;

            for (int i = 0; i < text.Length; ++i)
            {
                if (draw = GetCharacterSourceRect(text[i], ref sourceRect))
                {
                    batch.Draw(_texture, destRect, sourceRect, color);
                }

                if (draw || text[i] == ' ')
                {
                    destRect.X += _characterWidth * _scale + _spacing;
                }
            }
        }
    }
}
