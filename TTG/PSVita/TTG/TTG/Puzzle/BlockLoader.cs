using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class BlockLoader
    {
        Texture2D blockSpritesheet;
        Rectangle[] sheetRect = new Rectangle[6];
        const int blockSize = 51;

        public BlockLoader(Texture2D blocks)
        {
            blockSpritesheet = blocks;
        }

        public Rectangle GetBlockRect(int blockID)
        {
            return new Rectangle(blockID * blockSize, 0, blockSize, blockSize);
        }
    }
}
