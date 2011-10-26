//Gunship Class
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
    public class Gunship : Unit
    {
        public override void Initialize(int startX, int startY, Texture2D idleTex, Texture2D moveTex, Texture2D atkTex)
        {
            HP = 100;
            Attack = 20;
            Range = 25;
            Speed = 15;
            EnergyCost = 75;
            base.Initialize(startX, startY, idleTex, moveTex, atkTex);
        }
    }
}
