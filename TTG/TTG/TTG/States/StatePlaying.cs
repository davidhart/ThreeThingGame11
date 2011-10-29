using System;
using System.Collections.Generic;
using System.Linq;
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

        Arena arena;
        UI arenaUI;
        float elapsed;

        public StatePlaying(Game1 parent)
            : base(parent)
        {

        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            elapsed = 0;
            _graphics = graphics;
            spriteBatch = new SpriteBatch(graphics);

            arena = new Arena(1280, 200);
            arena.LoadContent(content, graphics);
            for (int i = 0; i < 7; ++i)
            {
                arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                arena.AddUnit(UnitEnum.Ember, UnitTeam.Player2);
            }
            arenaUI = new UI();
            arenaUI.Load(content, arena);

            // Puzzle grid set up
            _puzzleGrid = new PuzzleGrid(8, 8, graphics.PresentationParameters.BackBufferWidth / 2 - 64 * 4, 16);
            _puzzleGrid.LoadContent(content, graphics);
            _puzzleGrid.PopulateGrid();
        }

        public override void Reset()
        {
            elapsed = 0;
            arena.Reset();
            _puzzleGrid.PopulateGrid();
            arenaUI.SetBases(arena.GetBase1(), arena.GetBase2());
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            const float chancePerSec = 2;
            const float probabliltyOfSpawn = 0.4f;
            while (elapsed > 1 / chancePerSec)
            {
                if (Util.RandDouble() < probabliltyOfSpawn)
                {
                    //arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                }

                if (Util.RandDouble() < probabliltyOfSpawn)
                {
                    arena.AddUnit(UnitEnum.Ember, UnitTeam.Player2);
                }

                elapsed -= 1 / chancePerSec;
            }

            arena.Update(gameTime);
            arenaUI.Update(newMouse, oldMouse);

            // Puzzle
            _puzzleGrid.Update(gameTime, newMouse, oldMouse);

            if (arena.GetBase1().IsDead())
                ChangeScreen(_parent.GameOverState);
        }

        public override void Draw()
        {
            arena.Draw(spriteBatch);
            arena.DrawOntoScreen(new Vector2(0, _graphics.PresentationParameters.BackBufferHeight - arena.DisplayHeight));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            arenaUI.Draw(spriteBatch);
            spriteBatch.End();

            // Draw puzzle element
            _puzzleGrid.Draw(spriteBatch);
        }

    }
}
