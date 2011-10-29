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
    public class GameScreen
    {

        public virtual void Load(ContentManager content)
        {
        }
        public virtual void Draw(SpriteBatch spritebatch)
        {
        }
        public virtual void Update(MouseState newMouse, MouseState oldMouse)
        {

        }
    }
}
