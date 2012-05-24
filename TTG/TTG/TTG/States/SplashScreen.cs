using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TTG
{
    public class SplashScreen : GameState
    {
        Texture2D SplashTex;
        SpriteBatch _spriteBatch;
        Stopwatch stopwatch;

        public SplashScreen(Game1 parent)
            : base(parent)
        {
            _parent = parent;
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            _spriteBatch = new SpriteBatch(graphics);
            SplashTex = content.Load<Texture2D>("BHG");
            base.Load(content, graphics);
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
        public override void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(SplashTex, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            base.Draw();
        }
        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            if (stopwatch.ElapsedMilliseconds > 3000)
            {
                //change game state
                ChangeScreen(_parent.TitleScreenState);
            }
            base.Update(gameTime, newMouse, oldMouse);
        }
    }
}
