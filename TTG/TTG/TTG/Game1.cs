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

        public enum State
        {
            Menu,
            Playing
        }

        public State GameState = State.Playing;

        Arena arena;
        Animation marineMove;

        UI arenaUI;

        Random rand;

        float elapsed = 0;

        MouseState oldMouseState, newMouseState;

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
            this.IsMouseVisible = true;

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
            arenaUI = new UI();
            arenaUI.Load(Content, arena);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            newMouseState = Mouse.GetState();
            switch (GameState)
            {
                case State.Menu:
                    {
                        break;
                    }
                case State.Playing:
                    {
                        elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        const float chancePerSec = 2;
                        const float probabliltyOfSpawn = 0.4f;
                        while (elapsed > 1 / chancePerSec)
                        {
                            if (rand.NextDouble() < probabliltyOfSpawn)
                            {
                                //arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                            }

                            if (rand.NextDouble() < probabliltyOfSpawn)
                            {
                                arena.AddUnit(UnitEnum.Marine, UnitTeam.Player2);
                            }

                            elapsed -= 1 / chancePerSec;
                        }

                        arena.Update(gameTime);
                        arenaUI.Update(newMouseState, oldMouseState);
                        break;
                    }
                    
            }

            oldMouseState = newMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            switch (GameState)
            {
                case State.Menu:
                    {
                        break;
                    }
                case State.Playing:
                    {
                        arena.Draw(spriteBatch);
                        arenaUI.Draw(spriteBatch);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
