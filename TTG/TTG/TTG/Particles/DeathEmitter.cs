using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TTG
{
    public class DeathEmitter : PEmitter
    { 
        public DeathEmitter(Random rand)
            :base(rand)
        {
        }

        protected override void InitializeConstants(ContentManager cm, string texture)
        {
            // Init content
            base.LoadContent(cm, texture);

 	        // Data
            maxNumParticles = 20;

            minInitSpeed = 500.0f;
            maxInitSpeed = 700.0f;

            minAccel = 1000.0f;
            maxAccel = 1000.0f;

            minRotSpeed = -MathHelper.PiOver4 / 2.0f;
            maxRotSpeed = MathHelper.PiOver4 / 2.0f;

            minLife = 0.5f;
            maxLife = 1.0f;

            minSize = 2.0f;
            maxSize = 2.0f;

            blend = BlendState.AlphaBlend;

            timeBetweenRelease = 0.01f;

            cycleOnce = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
