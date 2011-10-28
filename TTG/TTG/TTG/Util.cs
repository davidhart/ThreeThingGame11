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


        private static Random rand = new Random();
    }
}
