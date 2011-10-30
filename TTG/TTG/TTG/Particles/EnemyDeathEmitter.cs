using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TTG
{
    public class EnemyDeathEmitter : PEmitter
    {
        public EnemyDeathEmitter(Random rand)
            : base(rand)
        {
        }

        protected override void InitializeConstants(ContentManager cm, string texture)
        {
            // Init content
            base.LoadContent(cm, texture);

            // Data
            maxNumParticles = 20;

            minInitSpeed = 100.0f;
            maxInitSpeed = 300.0f;

            minAccel = new Vector2(-100.0f, -100.0f);
            maxAccel = new Vector2(100.0f, 100.0f);

            minRotSpeed = -MathHelper.PiOver4 / 2.0f;
            maxRotSpeed = MathHelper.PiOver4 / 2.0f;

            minLife = 0.5f;
            maxLife = 1.0f;

            minSize = 0.3f;
            maxSize = 0.3f;

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
