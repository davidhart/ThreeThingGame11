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
        public override void Initialize(int startX, int startY, Texture2D idleTex, Texture2D moveTex, Texture2D atkTex)
        {
            HP = 20;
            Attack = 5;
            Range = 30;
            Speed = 10;
            EnergyCost = 10;
            base.Initialize(startX, startY, idleTex, moveTex, atkTex);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
