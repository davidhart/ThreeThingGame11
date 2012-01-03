using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TTG
{
    class GameOverState : GameState
    {
        float _elapsed;

        public GameOverState(Game1 parent)
            : base(parent)
        {
            Reset();
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            
        }

        public override void Reset()
        {
            _elapsed = 0;
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsed > 5.0f)
            {
                ChangeScreen(_parent.TitleScreenState);
            }
        }

        public override void Draw()
        {
            
        }
    }
}
