using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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

        // To replenish energy
        Arena _arena;

        // Font to draw score and combo multiplier
        SpriteFont _font;
        int _energy;
        int _combo;

        public PuzzleGrid(int gr, int gc, int xPos, int yPos, Arena arena)
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

            _arena = arena;

            _energy = 0;
            _combo = 0;

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

            _font = content.Load<SpriteFont>("UIFont");

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
                _energy = 0;
                _combo = 0;
                SetCursor();
                _energy *= _combo;
                _arena.P1Energy += _energy;
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
                    if (!_grid[row, col].Removed())
                        spriteBatch.Draw(_blockTexture[_grid[row, col].GetID()], new Rectangle(_x + col * 64, _y + row * 64, 64, 64), Color.White);
                }
            }

            // Draw selector if we have picked a tile to swap
            if (_cursorX != -1 && _cursorY != -1)
                spriteBatch.Draw(_blockSelection, new Rectangle(_x + _cursorX * 64, _y + _cursorY * 64, 64, 64), Color.White);

            spriteBatch.DrawString(_font, "Energy gained: ", new Vector2(_x + (64 * _columns), 10), Color.Black);
            spriteBatch.DrawString(_font, _energy.ToString(), new Vector2(_x + (64 * _columns) + 50, 50), Color.Black);

            if (_combo > 1)
            {
                spriteBatch.DrawString(_font, "(combo x" + _combo.ToString() + ")", new Vector2(_x + (64 * _columns) + 50, 100), Color.Black);
            }

            spriteBatch.End();
        }

        public void SetCursor()
        {
            int mx = ms.X - _x;
            int my = ms.Y - _y;

            int cellX = mx / 64;
            int cellY = my / 64;

            if (cellX < 0 || cellX >= _rows || cellY < 0 || cellY >= _columns)
                return; // If cell is out of bounds

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

                CheckGrid();
            }
            // Down
            else if (cellX == _cursorX && cellY == _cursorY + 1)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];
                _grid[_cursorY, _cursorX] = _grid[_cursorY + 1, _cursorX];
                _grid[_cursorY + 1, _cursorX] = blockTemp;

                CheckGrid();
            }
            // Left
            else if (cellX == _cursorX - 1 && cellY == _cursorY)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];

                _grid[_cursorY, _cursorX] = _grid[_cursorY, _cursorX - 1];
                _grid[_cursorY, _cursorX - 1] = blockTemp;

                CheckGrid();
            }
            // Right
            else if (cellX == _cursorX + 1 && cellY == _cursorY)
            {
                Block blockTemp = _grid[_cursorY, _cursorX];
                _grid[_cursorY, _cursorX] = _grid[_cursorY, _cursorX + 1];
                _grid[_cursorY, _cursorX + 1] = blockTemp;

                CheckGrid();
            }

            // Reset cursor
            _cursorX = -1;
            _cursorY = -1;
        }

        public void CheckGrid()
        {
            bool matches = false;

            // Solve grid until no more matches
            do
            {
                matches = false;

                // Mark any matches as 'removed'
                for (int r = 0; r < _rows; ++r)
                {
                    for (int c = 0; c < _columns; ++c)
                    {
                        if (!_grid[r, c].Removed())
                        {
                            matches |= CheckMatch(r, c);
                        }
                    }
                }

                bool changed = false;
                do
                {
                    // Move everything down 1 block
                    changed = false;
                    for (int row = _rows - 1; row > 0; row--)
                    {
                        for (int col = 0; col < _columns; col++)
                        {
                            if (_grid[row, col].Removed())
                            {
                                _grid[row, col] = new Block(_grid[row - 1, col].GetID(), _grid[row - 1, col].Removed());//_grid[row, col];
                                _grid[row - 1, col].Remove();
                                changed = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    // Replace the removed blocks on the top row with new blocks
                    for (int col = 0; col < _columns; col++)
                    {
                        if (_grid[0, col].Removed())
                        {
                            _grid[0, col] = new Block(Util.Rand(5));
                        }
                    }
                } while (changed);
            } while (matches);
        }

        public bool CheckMatch(int r, int c)
        {
            int id = _grid[r, c].GetID();

            List<Block> xMatches = new List<Block>();
            xMatches.Add(_grid[r, c]);
            List<Block> yMatches = new List<Block>();
            yMatches.Add(_grid[r, c]);

            // Check Left
            for (int x = c - 1; x >= 0; --x)
            {
                if (id == _grid[r, x].GetID())
                {
                    xMatches.Add(_grid[r, x]);
                }
                else
                    break;
            }

            // Check Right
            for (int x = c + 1; x < _columns; ++x)
            {
                if (id == _grid[r, x].GetID())
                {
                    xMatches.Add(_grid[r, x]);
                }
                else
                    break;
            }

            // Check Up
            for (int y = r - 1; y >= 0; --y)
            {
                if (id == _grid[y, c].GetID())
                {
                    yMatches.Add(_grid[y, c]);
                }
                else
                    break;
            }

            // Check Down
            for (int y = r + 1; y < _rows; ++y)
            {
                if (id == _grid[y, c].GetID())
                {
                    yMatches.Add(_grid[y, c]);
                }
                else
                    break;
            }

            bool matches = false;

            if (xMatches.Count >= 3)
            {
                int blockEnergy = 0;
                foreach (Block b in xMatches)
                {
                    blockEnergy = b.GetEnergy();
                    b.Remove();
                    
                }
                _combo++;
                _energy += blockEnergy * xMatches.Count;
                Debug.WriteLine("Combo: " + _combo);
                Debug.WriteLine("Energy: " + _energy);
                //_arena.P1Energy += _energy;

                matches = true;
            }

            if (yMatches.Count >= 3)
            {
                int blockEnergy = 0;
                foreach (Block b in yMatches)
                {
                    blockEnergy = b.GetEnergy();
                    b.Remove();
                    
                }
                _combo++;
                _energy += blockEnergy * yMatches.Count;
                Debug.WriteLine("Combo: " + _combo);
                Debug.WriteLine("Energy: " + _energy);
                //_arena.P1Energy += _energy;

                matches = true;
            }

            return matches;
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
