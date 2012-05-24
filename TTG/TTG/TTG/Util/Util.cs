using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TTG
{
    class Util
    {
        public static int Rand(int max)
        {
            return rand.Next(max);
        }

        public static int Rand()
        {
            return rand.Next();
        }

        public static double RandDouble()
        {
            return rand.NextDouble();
        }

        public static Vector2 RandVector(float xMax, float yMax)
        {
            return new Vector2((float)(rand.NextDouble() * xMax), (float)(rand.NextDouble() * yMax));
        }

        public static float Clamp(float i, float min, float max)
        {
            return Math.Max(Math.Min(i, max), min);
        }

        public static float Clamp(int i, int min, int max)
        {
            return Math.Max(Math.Min(i, max), min);
        }

        private static Random rand = new Random();

        public static Color ColorFromHSV(int hue, int sat, int val)
        {
            int r, g, b;
            int p, q, t;

            if (sat == 0)
            {
                r = g = b = val;
            }
            else
            {
                int f;
                f = ((hue % 60) * 255) / 60;
                hue /= 60;

                p = (val * (256 - sat)) / 256;
                q = (val * (256 - (sat * f) / 256)) / 256;
                t = (val * (256 - (sat * (256 - f)) / 256)) / 256;

                switch (hue)
                {
                    case 0:
                        r = val;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = val;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = val;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = val;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = val;
                        break;
                    default:
                        r = val;
                        g = p;
                        b = q;
                        break;
                }
            }

            return new Color(r, g, b);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return (1 - t) * a + t * b;
        }

        public static void PostToTwitter(string inStatus)
        {
            
        }
    }
}
