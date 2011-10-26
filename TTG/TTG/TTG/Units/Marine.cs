//Marine Class
//Lindsay Cox
//Last Updated 26/10/11

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
    public class Marine : Unit
    {
        public Marine(Vector2 position, Animation moveAnimation, Animation attackAnimation) : 
            base(position)
        {
            PlayAnimation(moveAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
