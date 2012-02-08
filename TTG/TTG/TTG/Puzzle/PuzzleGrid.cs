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

        const int blockSize = 51;
        const int padding = 8;
        const int blockStride = blockSize + padding;

        BitmapFont _bmFont;
        FancyText _fancyText;

        Vector2 _drawPosition;

        int _cursorX;
        int _cursorY;

        GraphicsDevice _graphics;

        Texture2D[] _blockTexture;
        Texture2D[] _blockTextureL;

        Texture2D _blockSelection;

        // To replenish energy
        Arena _arena;

        float _idleTime;

        // Font to draw score and combo multiplier
        SpriteFont _font;
        int _energy;
        int _combo;

        bool _matches;

        float _countDown;

        float _fallMaxDist;
        float _fallTime;

        const float _fallAnimationLength = 0.75f;
        const float _fadeOutTime = 0.3f;
        const float _shimmerEffectWidth = 1.75f;
        const float _shimmerAnimationLength = 1.8f;
        const float _shimmerAnimationStart = 4.5f;

        public PuzzleGrid(int gr, int gc, Vector2 drawPosition, Arena arena)
        {
            _bmFont = new BitmapFont("font", 37, 4);
            _bmFont.SetSpacing(-1);

            _fancyText = new FancyText(_bmFont);
            _fancyText.SetMessage("TESTx", new HighlightTextColorer(Color.Red));

            // Location to start drawing on screen
            _drawPosition = drawPosition;

            // Set up the grid
            _rows = gr;
            _columns = gc;

            _grid = new Block[_rows, _columns];

            _arena = arena;

            Reset();
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

        public void Reset()
        {
            _cursorX = -1;
            _cursorY = -1;

            _energy = 0;
            _combo = 0;

            _countDown = 0;
            _matches = false;

            _idleTime = 0;
            _fallMaxDist = 0;
            _fallTime = 0;

            PopulateGrid();
        }

        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            _graphics = device;

            _bmFont.LoadContent(content);

            _font = content.Load<SpriteFont>("UIFont");

            // Load block images here
            _blockTexture = new Texture2D[5];

            _blockTexture[0] = content.Load<Texture2D>("Block1");
            _blockTexture[1] = content.Load<Texture2D>("Block2");
            _blockTexture[2] = content.Load<Texture2D>("Block3");
            _blockTexture[3] = content.Load<Texture2D>("Block4");
            _blockTexture[4] = content.Load<Texture2D>("Block5");

            _blockTextureL = new Texture2D[5];
            _blockTextureL[0] = content.Load<Texture2D>("Block1l");
            _blockTextureL[1] = content.Load<Texture2D>("Block2l");
            _blockTextureL[2] = content.Load<Texture2D>("Block3l");
            _blockTextureL[3] = content.Load<Texture2D>("Block4l");
            _blockTextureL[4] = content.Load<Texture2D>("Block5l");

            _blockSelection = content.Load<Texture2D>("TileSelection");
        }

        public void Update(GameTime gameTime, MouseState currentMouseState, MouseState oldMouseState)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            _fancyText.Update(dt);

            if (_fallMaxDist > 0)
            {
                _fallTime += dt;

                if (_fallTime > _fallAnimationLength)
                {
                    _fallMaxDist = 0;
                    _fallTime = 0;

                    ResetBlockFallStates();
                }
            }
            
            if (_fallMaxDist == 0)
            {
                _idleTime += dt;
                _countDown -= dt;

                if (_countDown < 0)
                {
                    FancyTextColorer colorer;

                    if (_combo < 5)
                    {
                        Color c = Util.ColorFromHSV(360 - ((int)((_combo) / 3.0f * 360.0f + 16)), 255, 255);
                        colorer = new HighlightTextColorer(c);
                    }
                    else
                    {
                        colorer = new RainbowTextColorer();
                    }

                    if (_energy != 0)
                        _fancyText.SetMessage((_energy * _combo).ToString(), colorer);

                    SolveGrid();
                    CheckGrid();

                }

                if (_arena.P1Energy < _arena.MaxEnergy)
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed
                        && oldMouseState.LeftButton != ButtonState.Pressed && !_matches)
                    {
                        _idleTime = 0;
                        _energy = 0;
                        _combo = 0;

                        SetCursor(currentMouseState);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            if (_fallMaxDist > 0)
            {
                DrawBlocksWithOffsets(spriteBatch);
            }
            else
            {
                DrawBlocksIdle(spriteBatch);

                if (_matches && _fallMaxDist == 0)
                {
                    DrawRemovedBlockHighlight(spriteBatch);
                }
                else
                {
                    float t = CalculateShimmerTime();
                    if (t > 1.0f)
                    {
                        _idleTime = 0.0f;
                    }
                    else if (t > 0.0f)
                    {
                        DrawBlockShimmerHighlight(spriteBatch, t);
                    }
                }
            }

            // Draw selector if we have picked a tile to swap
            if (_cursorX != -1 && _cursorY != -1)
                spriteBatch.Draw(_blockSelection, new Rectangle((int)_drawPosition.X + _cursorX * 64, (int)_drawPosition.Y + _cursorY * 64, 64, 64), Color.White);

            if (_matches)
            {
                spriteBatch.DrawString(_font, "Energy gained: ", new Vector2(_drawPosition.X + (64 * _columns) + 20, 20), Color.White);
                spriteBatch.DrawString(_font, (_energy * _combo).ToString(), new Vector2(_drawPosition.X + (64 * _columns) + 70, 60), Color.White);

                if (_combo > 1)
                {
                    spriteBatch.DrawString(_font, "(combo x" + _combo.ToString() + ")", new Vector2(_drawPosition.X + (64 * _columns) + 70, 110), Color.White);
                }
            }
            else if (_arena.P1Energy >= _arena.MaxEnergy)
            {
                spriteBatch.DrawString(_font, "Energy Maxed Out", new Vector2(_drawPosition.X + (64 * _columns) + 20, 20), Color.White);
            }

            spriteBatch.End();


            _fancyText.Draw(spriteBatch, new Vector2(_drawPosition.X, _drawPosition.Y) + new Vector2(64 * _columns) / 2.0f - _fancyText.GetSize() / 2.0f);
        }

        /// <summary>
        /// Randomly place blocks on the grid, so that the same block type
        /// is not placed next to each other.
        /// </summary>
        private void PopulateGrid()
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
                        _grid[row, col].FallDistance = col;

                        isBlockOK = true;
                    }
                }
            }
            _fallTime = 0;
            _fallMaxDist = _rows;
        }

        private void ResetBlockFallStates()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    _grid[row, col].FallDistance = 0;
                }
            }
        }

        /// <summary>
        /// Calculates bounce height at the specified time. The returned offset at x=0 is 1 and the offset at
        /// x=1 is 0, for values between lie on a set of arcs.
        /// </summary>
        /// <param name="x">Value between 0 and 1</param>
        /// <returns>Offset between 1 and 0</returns>
        private float CalculateBounceHeight(float x)
        {
            float x2 = x-0.75f;
            return Math.Max(-x * x * 4 + 1,
                            -x2 * x2 * 16 * 0.3f + 0.3f);
        }

        private void DrawBlocksWithOffsets(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    float fallOffset = 0;

                    if (_grid[row, col].FallDistance != 0)
                    {
                        // Calculate clamped time offset
                        float d = _fallMaxDist * (_fallTime / _fallAnimationLength);
                        d = Math.Min(Math.Max(_grid[row, col].FallDistance - d, 0.0f) / _grid[row, col].FallDistance, 1.0f);

                        d = CalculateBounceHeight(1 - d);

                        // Calculate tile offset from normalized bounce height
                        fallOffset = (-_grid[row, col].FallDistance + (1 - d) * _grid[row, col].FallDistance);
                    }

                    spriteBatch.Draw(_blockTexture[_grid[row, col].GetID()], new Rectangle((int)_drawPosition.X + col * blockStride, 
                        (int)(_drawPosition.Y + (row + fallOffset) * blockStride), blockSize, blockSize), Color.White);
                }
            }
        }

        private void DrawBlocksIdle(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    spriteBatch.Draw(_blockTexture[_grid[row, col].GetID()], new Rectangle((int)_drawPosition.X + col * blockStride, 
                        (int)_drawPosition.Y + row * blockStride, blockSize, blockSize), Color.White);
                }
            }
        }

        private void DrawRemovedBlockHighlight(SpriteBatch spriteBatch)
        {
            float alpha = (float)Math.Sin((1.0f - _countDown / _fadeOutTime) * Math.PI);
            alpha = alpha * alpha;
           
            Color color = new Color(alpha, alpha, alpha, alpha);
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    if (_grid[row, col].Removed())
                    {
                        spriteBatch.Draw(_blockTextureL[_grid[row, col].GetID()], new Rectangle((int)_drawPosition.X + col * blockStride, 
                            (int)_drawPosition.Y + row * blockStride, blockSize, blockSize), color);
                    }
                }
            }
        }

        private void DrawBlockShimmerHighlight(SpriteBatch spriteBatch, float normalizedTime)
        {
            float shimmer = (normalizedTime * (1 + _shimmerEffectWidth * 2) - _shimmerEffectWidth) * (_columns + _rows);

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    if (!_grid[row, col].Removed())
                    {
                        float alpha = Math.Max(Math.Min(1 / Math.Abs(col + row - shimmer) * _shimmerEffectWidth - 0.35f, 1), 0);

                        spriteBatch.Draw(_blockTextureL[_grid[row, col].GetID()], new Rectangle((int)_drawPosition.X + col * blockStride, 
                            (int)_drawPosition.Y + row * blockStride, blockSize, blockSize), new Color(alpha, alpha, alpha, alpha));
                    }
                }
            }
        }

        private float CalculateShimmerTime()
        {
            return (_idleTime - _shimmerAnimationStart) / _shimmerAnimationLength;
        }

        private void SetCursor(MouseState mouseState)
        {
            int mx = mouseState.X - (int)_drawPosition.X;
            int my = mouseState.Y - (int)_drawPosition.Y;

            int cellX = mx / blockStride;
            int cellY = my / blockStride;

            // If cell is out of bounds abort
            if (cellX < 0 || cellX >= _rows || cellY < 0 || cellY >= _columns)
                return; 

            // If we have no previous selection, update the selection
            if (_cursorX == -1 && _cursorY == -1)
            {
                _cursorX = cellX;
                _cursorY = cellY;
                return;
            }

            // Otherwise if we have a second selection check if it is adjacent
            int [] xOffsets = { 0, 0, 1, -1};
            int [] yOffsets = { -1, 1, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                if (cellX == _cursorX + xOffsets[i] && cellY == _cursorY + yOffsets[i])
                {
                    Block blockTemp = _grid[_cursorY, _cursorX];
                    _grid[_cursorY, _cursorX] = _grid[_cursorY + yOffsets[i], _cursorX + xOffsets[i]];
                    _grid[_cursorY + yOffsets[i], _cursorX + xOffsets[i]] = blockTemp;

                    CheckGrid();
                    break;
                }
            }

            // Reset cursor
            _cursorX = -1;
            _cursorY = -1;
        }

        private void CheckGrid()
        {
            // Solve grid until no more matches
            _matches = false;

            // Mark any matches as 'removed'
            for (int r = 0; r < _rows; ++r)
            {
                for (int c = 0; c < _columns; ++c)
                {
                    if (!_grid[r, c].Removed())
                    {
                        CheckMatch(r, c);
                    }
                }
            }

            if (_matches)
                _countDown = _fadeOutTime;
            else
            {
                _arena.P1Energy += _energy * _combo;
                _energy = 0;
                _combo = 0;
            }
        }

        private void SolveGrid()
        {
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
                            _grid[row, col] = new Block(_grid[row - 1, col]);
                            _grid[row - 1, col].Remove();
                            _grid[row, col].FallDistance = _grid[row, col].FallDistance + 1;
                            _fallMaxDist = Math.Max(_grid[row, col].FallDistance, _fallMaxDist);

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

                        if (_grid[1, col].FallDistance > 0)
                            _grid[0, col].FallDistance = _grid[1, col].FallDistance;
                        else
                            _grid[0, col].FallDistance = 1;

                        _fallMaxDist = Math.Max(_grid[0, col].FallDistance, _fallMaxDist);
                    }
                }
            } while (changed);
        }

        private void CheckMatch(int r, int c)
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

            // Increase the combo multiplier if this is the first match this solve step
            if ((xMatches.Count >= 3 || yMatches.Count >= 3) && !_matches)
            {
                _combo++;
                _matches = true;
            }

            ProcessMatchesLine(xMatches);
            ProcessMatchesLine(yMatches);
        }

        private void ProcessMatchesLine(List<Block> line)
        {
            if (line.Count >= 3)
            {
                int blockEnergy = 0;
                foreach (Block b in line)
                {
                    if (!b.Removed())
                    {
                        _energy += b.GetEnergy();

                        b.Remove();
                    }
                }

                _energy += blockEnergy * line.Count;
            }
        }
    }
}