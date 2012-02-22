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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GameState _currentState;

        StatePlaying _playingState;
        public GameState PlayState
        {
            get
            {
                return _playingState;
            }
        }

        TitleScreen _titlescreen;
        public GameState TitleScreenState
        {
            get
            {
                return _titlescreen;
            }
        }

        GameOverState _gameOverSate;
        public GameState GameOverState
        {
            get
            {
                return _gameOverSate;
            }
        }

        GraphicsDeviceManager graphics;

        HelpScreen _helpScreen;
        public GameState HelpScreen
        {
            get
            {
                return _helpScreen;
            }
        }
        MouseState oldMouseState, newMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _playingState = new StatePlaying(this);
            _playingState.Load(Content, GraphicsDevice);

            _titlescreen = new TitleScreen(this);
            _titlescreen.Load(Content, GraphicsDevice);

            _gameOverSate = new GameOverState(this);
            _gameOverSate.Load(Content, GraphicsDevice);

            _helpScreen = new HelpScreen(this);
            _helpScreen.Load(Content, GraphicsDevice);

            _currentState = _titlescreen;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            newMouseState = Mouse.GetState();

            _currentState.Update(gameTime, newMouseState, oldMouseState);

            oldMouseState = newMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _currentState.Draw();

            base.Draw(gameTime);
        }

        public void ChangeState(GameState state)
        {
            _currentState = state;
            _currentState.Reset();
        }
    }
}
