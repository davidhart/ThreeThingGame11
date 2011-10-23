//Base Unit Class
//Lindsay Cox
//Last Updated 23/10/11

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
    public class Unit
    {
        public Rectangle UnitRect;

        Texture2D unitTex;

        public virtual void Update()
        {
            Animate();
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(unitTex, UnitRect, Color.White);
        }

        
    }
}
