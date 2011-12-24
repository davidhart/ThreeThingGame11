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
        int _fallDistance;
        const int _energy = 10;

        public int FallDistance
        {
            get { return _fallDistance; }
            set { _fallDistance = value; }
        }

        public Block(int id)
        {
            _id = id;
            _fallDistance = 0;
            _remove = false;
        }

        public Block(Block block)
        {
            _id = block._id;
            _remove = block._remove;
            _fallDistance = block._fallDistance;
        }

        public int GetID()
        {
            return _id;
        }

        public int GetEnergy()
        {
            return _energy;
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
