//Animation Class
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
    public class Animation
    {
        int frameNumber = 0;
        float frameTimer = 0.0f;
        float frameSpeed = (1.0f / 90.0f);

        Texture2D walkUpTex, walkDownTex, attackTex, idleTex;

        public Animation(Texture2D upSpSheet, Texture2D dwnSpSheet,
            Texture2D atkSpSheet, Texture2D idleSpSheet)
        {
            walkUpTex = upSpSheet;
            walkDownTex = dwnSpSheet;
            attackTex = atkSpSheet;
            idleTex = idleSpSheet;
        }

        enum AnimateState
        {
            WalkingUp,
            WalkingDown,
            Attack,
            Idle
        }

        AnimateState animationState = AnimateState.Idle;

        void SetupAnimation()
        {

        }

        Texture2D CurrentFrame()
        {
            Texture2D currentFrame;
            return currentFrame;
        }
    }
}
