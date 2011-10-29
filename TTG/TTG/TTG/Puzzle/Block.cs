using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    class Block
    {
        int _id;

        public Block(int id)
        {
            _id = id;
        }

        public int GetID()
        {
            return _id;
        }
    }
}
