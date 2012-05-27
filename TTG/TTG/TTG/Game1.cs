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

        SplashScreen _splashScreen;
        public GameState SplashScreen
        {
            get
            {
                return _splashScreen;
            }
        }

        TimeAttack _timeAttack;
        public GameState TimeAttack
        {
            get
            {
                return _timeAttack;
            }
        }

        WorldMap _campaignMap;
        public GameState CampaignMap
        {
            get
            {
                return _campaignMap;
            }
        }

        Cutscene _introCutscene;
        public GameState IntroCutscene
        {
            get
            {
                return _introCutscene;
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

            _splashScreen = new SplashScreen(this);
            _splashScreen.Load(Content, GraphicsDevice);

            _timeAttack = new TimeAttack(this);
            _timeAttack.Load(Content, GraphicsDevice);

            _campaignMap = new WorldMap(this);
            _campaignMap.Load(Content, GraphicsDevice);

            SetupCutscenes();

            _currentState = _splashScreen;
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

        void SetupCutscenes()
        {
            _introCutscene = new Cutscene(this);
            _introCutscene.Load(Content, GraphicsDevice);
            Texture2D intro1 = Content.Load<Texture2D>("Intro1");
            Texture2D intro2 = Content.Load<Texture2D>("Intro2");
            _introCutscene.AddFrame(intro1, "Two hours ago, the Earth Council received",
                "a message from the terrorist organisation", "known as Zeno.");
            _introCutscene.AddFrame(intro1, "The message stated that if the Earth Council does not",
                "surrender their imprisoned leaders, they will attack", "various Earth colonies around the galaxy");
            _introCutscene.AddFrame(intro1, "The guys down at Communications managed to track down",
                "the source of the message to Volcanis, a fiery planet", "situated in the delta quadrant");
            _introCutscene.AddFrame(intro2, "We believe that the remaining Zeno generals are on Volcanis,",
                "harnessing the planets abundant resources", "to build an army of fire warriors");
            _introCutscene.AddFrame(intro2, "Your mission, Commander is for you and your unit",
                "to travel to Volcanis, capture the remaining Zeno generals", "and destroy the factories creating these fire warriors");
            _introCutscene.AddFrame(intro2, "That is all.", "Good Luck Commander", "");
        }
    }
}
