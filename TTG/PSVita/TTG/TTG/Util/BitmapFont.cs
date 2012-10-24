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

        public BitmapFont(string fontName, int charactersX, int charactersY)
        {
            _name = fontName;
            _charactersX = charactersX;
            _charactersY = charactersY;
            _spacing = 0;
            _characterWidth = 0;
            _characterHeight = 0;
        }

        public void SetSpacing(int spacing)
        {
            _spacing = spacing;
        }

        public Vector2 GetCharSize()
        {
            return new Vector2(_characterWidth, _characterHeight);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("fonts/" + _name);
            _characterWidth = _texture.Width / _charactersX;
            _characterHeight = _texture.Height / _charactersY;
        }

        public void DrawText(SpriteBatch batch, string text, Vector2 position, float scale)
        {
            DrawText(batch, text, position, Color.White, scale, 0);
        }

        private void GetSourceRectFromIndex(int index, ref Rectangle r)
        {
            r = new Rectangle((index % _charactersX) * _characterWidth, 
                              (index / _charactersX) * _characterHeight, 
                              _characterWidth, 
                              _characterHeight);
        }

        private bool GetCharacterSourceRect(char character, ref Rectangle r, int characterOffset)
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

            GetSourceRectFromIndex(charIndex + characterOffset, ref r);

            return true;
        }

        public void DrawText(SpriteBatch batch, string text, Vector2 position, Color color, float scale, int characterOffset)
        {
            Rectangle sourceRect = new Rectangle();
            Rectangle destRect = new Rectangle(0, 0, (int)(_characterWidth * scale), (int)(_characterHeight * scale));
            bool draw = false;

            for (int i = 0; i < text.Length; ++i)
            {
                if (draw = GetCharacterSourceRect(text[i], ref sourceRect, characterOffset))
                {
                    destRect.X = (int)position.X;
                    destRect.Y = (int)position.Y;
                    batch.Draw(_texture, destRect, sourceRect, color);
                }

                if (draw || text[i] == ' ')
                {
                    position.X += (_characterWidth + _spacing) * scale;
                }
            }
        }
    }
}
