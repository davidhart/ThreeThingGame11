using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TTG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Arena arena;
        Animation marineMove;

        Random rand;

        float elapsed = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;

            rand = new Random();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            marineMove = new Animation(Content.Load<Texture2D>("marine"), 3, 1, 0, 3, 0.05f, true);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arena = new Arena();
            arena.LoadContent(Content);
            for (int i = 0; i < 7; ++i)
            {
                arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                arena.AddUnit(UnitEnum.Marine, UnitTeam.Player2);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            const float chancePerSec = 2;
            const float probabliltyOfSpawn = 0.2f;
            while (elapsed > 1 / chancePerSec)
            {
                if (rand.NextDouble() < probabliltyOfSpawn)
                {
                    arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                }

                if (rand.NextDouble() < probabliltyOfSpawn)
                {
                    arena.AddUnit(UnitEnum.Marine, UnitTeam.Player2);
                }

                elapsed -= 1 / chancePerSec;
            }

            arena.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            arena.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
