using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TTG
{
    public class GameState
    {
        protected Game1 _parent;

        public GameState(Game1 parent)
        {
            _parent = parent;
        }

        public virtual void Load(ContentManager content, GraphicsDevice graphics)
        {

        }

        public virtual void Reset()
        {

        }

        public virtual void Draw()
        {

        }

        public virtual void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {

        }

        public void ChangeScreen(GameState newState)
        {
            _parent.ChangeState(newState);
        }
    }
}
