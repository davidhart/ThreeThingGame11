using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using System.Diagnostics;

namespace TTG
{
    class PuzzleGrid
    {
        Block[,] _grid;
        int _rows;
        int _columns;

        int _x;
        int _y;

        public PuzzleGrid(int gr, int gc, int xPos, int yPos)
        {
            // Location to start drawing on screen
            _x = xPos;
            _y = yPos;

            // Set up the grid
            _rows = gr;
            _columns = gc;

            _grid = new Block[_rows, _columns];

            PopulateGrid();
        }

        /// <summary>
        /// Randomly place blocks on the grid, so that the same block type
        /// is not placed next to each other.
        /// </summary>
        public void PopulateGrid()
        {
            Random rand = new Random();

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    bool isBlockOK = false;

                    while (isBlockOK == false)
                    {
                        int blockType = rand.Next(5);

                        // Check the block type in the row above
                        if (row > 0)
                        {
                            if (_grid[row - 1, col].GetID() == blockType)
                            {
                                continue;
                            }
                        }

                        // Check the block type in the column to the left
                        if (col > 0)
                        {
                            if (_grid[row, col - 1].GetID() == blockType)
                            {
                                continue;
                            }
                        }
                        
                        _grid[row, col] = new Block(blockType);

                        isBlockOK = true;
                    }
                }
            }
        }

        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            // Load block images here
        }

        public void Update(GameTime gameTime)
        {
            // Player controls here
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw blocks in the grid here
        }

        public override string ToString()
        {
            string output = "";

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    output += _grid[row, col].GetID() + ",";
                }
                output += "\n";
            }
            return output;
        }
    }
}
