using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class Fire : PSystem
    {
        public Fire(Game1 game, int numEffects)
            : base(game, numEffects)
        {
        }

        protected override void InitializeConstants()
        {
            textureFilename = "fire";

            minInitialSpeed = 0;
            maxInitialSpeed = 0;

            minAcceleration = 5.0f;
            maxAcceleration = 7.0f;

            minLifetime = 0.5f;
            maxLifetime = 4.0f;

            minScale = 0.5f;
            maxScale = 1.0f;

            minNumParticles = 10;
            maxNumParticles = 20;

            minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            maxRotationSpeed = MathHelper.PiOver4 / 2.0f;

            blendState = BlendState.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;
        }
    }
}
