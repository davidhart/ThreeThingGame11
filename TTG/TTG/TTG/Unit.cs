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
        public Texture2D UnitTex;
        public enum Type
        {
            Light,
            Heavy,
            Ranged,
            Aerial,
            Hero
        }

        public Type UnitType;

        public void Update()
        {
            switch (UnitType)
            {
                case Type.Aerial:
                    {
                        break;
                    }
                case Type.Heavy:
                    {
                        break;
                    }
                case Type.Hero:
                    {
                        break;
                    }
                case Type.Light:
                    {
                        break;
                    }
                case Type.Ranged:
                    {
                        break;
                    }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
        }
    }
}
