﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        // Position where to start drawing the grid
        int _x;
        int _y;

        int _cursorX;
        int _cursorY;

        MouseState ms;

        GraphicsDevice _graphics;

        Texture2D[] _blockTexture;

        Texture2D _blockSelection;

        public PuzzleGrid(int gr, int gc, int xPos, int yPos)
        {
            // Location to start drawing on screen
            _x = xPos;
            _y = yPos;

            // Set up the grid
            _rows = gr;
            _columns = gc;

            _grid = new Block[_rows, _columns];

            _cursorX = -1;
            _cursorY = -1;

            //PopulateGrid();
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
            _graphics = device;

            // Load block images here
            _blockTexture = new Texture2D[5];

            _blockTexture[0] = content.Load<Texture2D>("Block1");
            _blockTexture[1] = content.Load<Texture2D>("Block2");
            _blockTexture[2] = content.Load<Texture2D>("Block3");
            _blockTexture[3] = content.Load<Texture2D>("Block4");
            _blockTexture[4] = content.Load<Texture2D>("Block5");

            _blockSelection = content.Load<Texture2D>("TileSelection");
        }

        public void Update(GameTime gameTime, MouseState currentMouseState, MouseState oldMouseState)
        {
            // Player controls here
            ms = currentMouseState;

            if (currentMouseState.LeftButton == ButtonState.Pressed 
                && oldMouseState.LeftButton != ButtonState.Pressed)
            {
                SetCursor();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
        
            // Draw blocks in the grid here
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    spriteBatch.Draw(_blockTexture[_grid[row, col].GetID()], new Rectangle(_x + col * 64, _y + row * 64, 64, 64), Color.White);
                }
            }

            // Draw selector if we have picked a tile to swap
            if (_cursorX != -1 && _cursorY != -1)
                spriteBatch.Draw(_blockSelection, new Rectangle(_cursorX * 64, _cursorY * 64, 64, 64), Color.White);

            spriteBatch.End();
        }

        public void SetCursor()
        {
            int mx = ms.X - _x;
            int my = ms.Y - _y;

            int cellX = mx / 64;
            int cellY = my / 64;

            if (_cursorX == -1 && _cursorY == -1)
            {
                _cursorX = cellX;
                _cursorY = cellY;
                return;
            }

            // Up
            if (cellX == _cursorX && cellY == _cursorY - 1)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];
                _grid[_cursorY, _cursorX] = _grid[_cursorY - 1, _cursorX];
                _grid[_cursorY - 1, _cursorX] = blockTemp;
            }
            // Down
            else if (cellX == _cursorX && cellY == _cursorY + 1)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];
                _grid[_cursorY, _cursorX] = _grid[_cursorY + 1, _cursorX];
                _grid[_cursorY + 1, _cursorX] = blockTemp;
            }
            // Left
            else if (cellX == _cursorX - 1 && cellY == _cursorY)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];

                _grid[_cursorY, _cursorX] = _grid[_cursorY, _cursorX - 1];
                _grid[_cursorY, _cursorX - 1] = blockTemp;
            }
            // Right
            else if (cellX == _cursorX + 1 && cellY == _cursorY)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];
                _grid[_cursorY, _cursorX] = _grid[_cursorY, _cursorX + 1];
                _grid[_cursorY, _cursorX + 1] = blockTemp;
            }

            // Reset cursor
            _cursorX = -1;
            _cursorY = -1;
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
