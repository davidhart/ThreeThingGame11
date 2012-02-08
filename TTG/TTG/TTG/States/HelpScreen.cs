using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TTG
{
    public class HelpScreen : GameState
    {
        Texture2D _screen;
        Rectangle _rect;
        SpriteBatch _spriteBatch;

        public HelpScreen(Game1 parent)
            :base(parent)
        {
            _parent = parent;
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            _rect = new Rectangle(0, 0, 1280, 768);
            _screen = content.Load<Texture2D>("HelpScreen");
            _spriteBatch = new SpriteBatch(graphics);
        }

        public override void Reset()
        {

        }

        public override void Draw()
        {
        _spriteBatch.Begin();
            _spriteBatch.Draw(_screen, _rect, Color.White);
            base.Draw();
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            if (newMouse.LeftButton == ButtonState.Pressed &&
                oldMouse.LeftButton == ButtonState.Released)
                ChangeScreen(_parent.TitleScreenState);
        }
    }
}
