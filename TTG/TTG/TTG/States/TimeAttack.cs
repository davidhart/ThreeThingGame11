using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TTG
{
    public class TimeAttack : GameState
    {
        PuzzleGrid _puzzleGrid;
        SpriteBatch _spriteBatch;
        GraphicsDevice _graphics;
        Stopwatch stopwatch, stopwatch2;
        SpriteFont _font;
        Arena arena;
        UI arenaUI;
        float elapsed;
        bool timerStarted = false;
        bool endTimer = false;

        public TimeAttack(Game1 parent)
            : base(parent)
        {
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            Rectangle UIRectangle = new Rectangle(0, 480, 480, 800 - 480);

            elapsed = 0;

            _graphics = graphics;
            _spriteBatch = new SpriteBatch(graphics);

            arena = new Arena(4810, 170, new Vector2(0, 800 - 200));
            arena.LoadContent(content, graphics);

            arenaUI = new UI(UIRectangle);
            arenaUI.Load(content, arena);

            // Puzzle grid set up
            _puzzleGrid = new PuzzleGrid(8, 8, new Vector2(8, 8), arena);
            _puzzleGrid.LoadContent(content, graphics);

            //stopwatch init
            stopwatch = new Stopwatch();
            stopwatch2 = new Stopwatch();

            _font = content.Load<SpriteFont>("UIFont");

        }

        public override void Reset()
        {
            elapsed = 0;
            arena.Reset();
            _puzzleGrid.Reset();
            arenaUI.SetBases(arena.GetBase1(), arena.GetBase2());
            endTimer = false;
            stopwatch.Reset();
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            if (!timerStarted)
            {
                stopwatch.Start();
                timerStarted = true;
            }

            arenaUI.isTimeAtk = true;
            arena.isTimeAtk = true;

            if (arena.GetBase1().IsDead())
            {
                if (!endTimer)
                {
                    stopwatch2.Start();
                    endTimer = false;
                }
                if (stopwatch2.ElapsedMilliseconds > 3000)
                {
                    ChangeScreen(_parent.TitleScreenState);
                }
            }
            else
            {
                //spawns units based on time
                elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                float chancePerSec = 1.0f
                   + (float)(stopwatch.ElapsedMilliseconds / 1000);
                float probabliltyOfSpawn = 0.3f
                    + (float)(stopwatch.ElapsedMilliseconds / 2000);

                while (elapsed > 1 / chancePerSec)
                {
                    if (Util.RandDouble() < probabliltyOfSpawn)
                    {
                        if (Util.Rand(5) == 0)
                            arena.AddUnit(UnitEnum.UberEmber, UnitTeam.Player2);
                        else
                            arena.AddUnit(UnitEnum.Ember, UnitTeam.Player2);
                    }

                    elapsed -= 1 / chancePerSec;
                }

                //Puzzle
                _puzzleGrid.Update(gameTime, newMouse, oldMouse);
                arenaUI.Update((float)gameTime.ElapsedGameTime.TotalSeconds, newMouse, oldMouse);
            }

            arena.Update(gameTime);
        }

        public override void Draw()
        {
            arena.Draw(_spriteBatch);

            arenaUI.Draw(_spriteBatch);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.DrawString(_font, "Time: "
                + stopwatch.ElapsedMilliseconds / 1000,
                new Vector2(480 - 145, 800 - 200), Color.White);
                
            if (arena.GetBase1().IsDead())
            {
                Vector2 text = _font.MeasureString("GAME OVER");
                _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(_graphics.PresentationParameters.BackBufferWidth / 2 - text.X / 2, _graphics.PresentationParameters.BackBufferHeight / 2 - text.Y * 2), Color.White);
                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.End();
                _puzzleGrid.Draw(_spriteBatch);
            }
        }
    }
}
