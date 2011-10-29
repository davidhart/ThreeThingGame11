using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class DeathEmitter : PEmitter
    {
        public DeathEmitter(Game1 game, Random rand, string textureFilename, SpriteBatch sb)
            : base(game, rand)
        {
            this.textureFilename = textureFilename;
            this.sb = sb;
        }

        protected override void InitializeConstants()
        {
 	        // Data
            maxNumParticles = 4;

            minInitSpeed = 1.0f;
            maxInitSpeed = 2.0f;

            minAccel = -1.0f;
            maxAccel = 0.0f;

            minRotSpeed = -MathHelper.PiOver4 / 2.0f;
            maxRotSpeed = MathHelper.PiOver4 / 2.0f;

            minLife = 1.0f;
            maxLife = 5.0f;

            blend = BlendState.AlphaBlend;

            timeBetweenRelease = 0.0f;
        }
    }
}
