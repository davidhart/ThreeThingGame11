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
        int _energy;

        public Block(int id)
        {
            _id = id;
            _remove = false;
            _energy = 5;
        }

        public Block(int id, bool remove)
        {
            _id = id;
            _remove = remove;
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
