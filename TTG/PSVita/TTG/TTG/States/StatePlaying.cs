﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TTG
{
    class StatePlaying : GameState
    {
        PuzzleGrid _puzzleGrid;
        SpriteBatch spriteBatch;
        GraphicsDevice _graphics;
        KeyboardState _prevKeyState;
        Stopwatch stopwatch;
        bool timerStarted = false;
        Arena arena;
        UI arenaUI;
        float elapsed;
        SpriteFont _font;

        public StatePlaying(Game1 parent)
            : base(parent)
        {

        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            Rectangle UIRectangle = new Rectangle(0, 480, 480, 800-480);

            elapsed = 0;
            _graphics = graphics;
            spriteBatch = new SpriteBatch(graphics);

            arena = new Arena(480, 170, new Vector2(0, 800-200));
            arena.LoadContent(content, graphics);
            
            /*
            for (int i = 0; i < 7; ++i)
            {
                arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                arena.AddUnit(UnitEnum.Ember, UnitTeam.Player2);
            }*/
            arenaUI = new UI(UIRectangle);
            arenaUI.Load(content, arena);

            // Puzzle grid set up
            _puzzleGrid = new PuzzleGrid(8, 8, new Vector2(8, 8), arena);
            _puzzleGrid.LoadContent(content, graphics);

            _font = content.Load<SpriteFont>("UIFont");

            _prevKeyState = Keyboard.GetState();

            stopwatch = new Stopwatch();
        }

        public override void Reset()
        {
            elapsed = 0;
            arena.Reset();
            _puzzleGrid.Reset();
            arenaUI.SetBases(arena.GetBase1(), arena.GetBase2());
            timerStarted = false;
            stopwatch.Reset();
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            arenaUI.isTimeAtk = false;
            arena.isTimeAtk = false;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ChangeScreen(_parent.TitleScreenState);
            }
            if (arena.GetBase1().IsDead())
            {
                if (!timerStarted)
                {
                    stopwatch.Start();
                    timerStarted = true;
                }
                if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    ChangeScreen(_parent.TitleScreenState);
                }
            }
            else if (arena.GetBase2().IsDead())
            {
                if (!timerStarted)
                {
                    stopwatch.Start();
                    timerStarted = true;
                }
                if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    ChangeScreen(_parent.TitleScreenState);
                }
            }
            else
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                const float chancePerSec = 1.0f;
                const float probabliltyOfSpawn = 0.3f;
                while (elapsed > 1 / chancePerSec)
                {
                    if (Util.RandDouble() < probabliltyOfSpawn)
                    {
                        //arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                    }

                    if (Util.RandDouble() < probabliltyOfSpawn)
                    {
                        if (Util.Rand(5) == 0)
                            arena.AddUnit(UnitEnum.UberEmber, UnitTeam.Player2);
                        else
                            arena.AddUnit(UnitEnum.Ember, UnitTeam.Player2);
                    }

                    elapsed -= 1 / chancePerSec;
                }

                // Puzzle
                _puzzleGrid.Update(gameTime, newMouse, oldMouse);
                arenaUI.Update((float)gameTime.ElapsedGameTime.TotalSeconds, newMouse, oldMouse);
            }

            arena.Update(gameTime);
        }

        public override void Draw()
        {
            arena.Draw(spriteBatch);

            arenaUI.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (arena.GetBase1().IsDead())
            {
                Vector2 text = _font.MeasureString("GAME OVER");
                spriteBatch.DrawString(_font, "GAME OVER", new Vector2(_graphics.PresentationParameters.BackBufferWidth / 2 - text.X / 2, _graphics.PresentationParameters.BackBufferHeight / 2 - text.Y * 2), Color.White);
                //text = _font.MeasureString("Touch to Continue");
                //spriteBatch.DrawString(_font, "Touch to Continue", new Vector2(_graphics.PresentationParameters.BackBufferWidth / 2 - text.X / 2, _graphics.PresentationParameters.BackBufferHeight / 2 - text.Y), Color.White);
                spriteBatch.End();
            }
            else if (arena.GetBase2().IsDead())
            {
                Vector2 text = _font.MeasureString("YOU WIN");
                spriteBatch.DrawString(_font, "YOU WIN", new Vector2(_graphics.PresentationParameters.BackBufferWidth / 2 - text.X / 2, _graphics.PresentationParameters.BackBufferHeight / 2 - text.Y * 2), Color.White);
                //text = _font.MeasureString("Touch to Continue");
                //spriteBatch.DrawString(_font, "Touch to Continue", new Vector2(_graphics.PresentationParameters.BackBufferWidth / 2 - text.X / 2, _graphics.PresentationParameters.BackBufferHeight / 2 - text.Y), Color.White);
                spriteBatch.End();
            }
            else
            {
                // Draw puzzle element
                spriteBatch.End();
                _puzzleGrid.Draw(spriteBatch);
            }
        }

    }
}
