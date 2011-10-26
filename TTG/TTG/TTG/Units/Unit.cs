//Base Unit Class
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
    public class Unit
    {
        #region Stats
        int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }
        }

        int atk;
        public int Attack
        {
            get
            {
                return atk;
            }
            set
            {
                atk = value;
            }
        }

        int range;
        public int Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
            }
        }

        int spd;
        public int Speed
        {
            get
            {
                return spd;
            }
            set
            {
                spd = value;
            }
        }

        int energy;
        public int EnergyCost
        {
            get
            {
                return energy;
            }
            set
            {
                energy = value;
            }
        }
        #endregion

        #region Animation and Sound
        Animation moveAnimation, attackAnimation;
        AnimationPlayer sprite;
        SoundEffect spawnSound, dieSound;
        #endregion

        bool isAlive;
        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;
            }
        }

        public Rectangle UnitRect;
        Vector2 pos;

        public virtual void Initialize(
            int startX,
            int startY,
            Texture2D idleTex,
            Texture2D moveTex,
            Texture2D atkTex)
        {
            UnitRect = new Rectangle(startX, startY, idleTex.Width, idleTex.Height);
            moveAnimation = new Animation(moveTex, 0.1f, true);
            attackAnimation = new Animation(atkTex, 0.1f, true);
        }

        public virtual void Update()
        {
            pos.X = UnitRect.X;
            pos.Y = UnitRect.Y;
        }

        public virtual void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            sprite.Draw(gametime, spritebatch, pos, SpriteEffects.None);
        }

        
    }
}
