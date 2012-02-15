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
        const int offsetX = 206;
        const int offsetY = 13;
        const int rows = 1;
        const int columns = 6;

        public BlockLoader(Texture2D blocks)
        {
            blockSpritesheet = blocks;
        }

        public Rectangle GetBlockRect(int blockID)
        {
            int width =  (blockSpritesheet.Width  - offsetX) / columns;
            int height = (blockSpritesheet.Height - offsetY) / rows;
            int x = (blockID % columns) * width;
            int y = (blockID / columns) * height;
            return new Rectangle(x, y, width, height);
        }
    }
}
