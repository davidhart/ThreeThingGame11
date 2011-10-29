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
        bool _remove;

        public Block(int id)
        {
            _id = id;
            _remove = false;
        }

        public int GetID()
        {
            return _id;
        }

        public void Remove()
        {
            _remove = true;
        }

        public bool Removed()
        {
            return _remove;
        }
    }
}
