﻿using System;
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
    public enum TitleState
    {
        Main,
        Campaign,
        Help,
        PlayGame
    }
    public class TitleButton
    {
        Texture2D _uiButtonTex, _uiButtonTexClick;
        SoundEffect _uiButtonSound;
        Rectangle _buttonRect, _mouseRect, rectang;

        public delegate void PressedEventHandler(object sender, EventArgs e);
        public event PressedEventHandler OnPress;

        public TitleButton(Texture2D buttontex, Texture2D buttonSelecttex,
            SoundEffect buttonSE, Rectangle buttonRect)
        {

            _uiButtonTex = buttontex;
            _uiButtonTexClick = buttonSelecttex;
            _uiButtonSound = buttonSE;
            _mouseRect = new Rectangle(0, 0, 1, 1);
            _buttonRect = buttonRect;

        }

        public void Update(MouseState newMousestate, MouseState oldMouseState)
        {
            _mouseRect.X = newMousestate.X;
            _mouseRect.Y = newMousestate.Y;

            if (_mouseRect.Intersects(_buttonRect) &&
                newMousestate.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released)
            {
                //_uiButtonSound.Play();
                OnPress.Invoke(this, null);

            }
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (_mouseRect.Intersects(_buttonRect))
            {
                spritebatch.Draw(_uiButtonTexClick, _buttonRect, Color.White);
            }
            else
            {
                spritebatch.Draw(_uiButtonTex, _buttonRect, Color.White);
            }
        }
    }
    public class TitleScreen : GameState
    {
        TitleState titleState = TitleState.Main;
        Texture2D _titleTex;
        Rectangle _titleRect;
        TitleButton _startBtn, _helpBtn, _quitBtn, _campaignBtn, _muteBtn, _taBtn;
        SpriteBatch _spriteBatch;

        public TitleScreen(Game1 parent)
            : base(parent)
        {

        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            //_titleRect = new Rectangle(0, 0, 1, 768);
            _titleTex = content.Load<Texture2D>("Titlephone");
            SoundEffect select = content.Load<SoundEffect>("Select");

            _campaignBtn = new TitleButton(
                content.Load<Texture2D>("CampaignBtn"),
                content.Load<Texture2D>("CampaignBtn"),
                select, new Rectangle(100, 170, 256, 128));
            _campaignBtn.OnPress += new TitleButton.PressedEventHandler(_campaignBtn_OnPress);

            _startBtn = new TitleButton(
                content.Load<Texture2D>("IAbtn"),
                content.Load<Texture2D>("IAbtn"),
                select, new Rectangle (100, 300, 256, 128));
            _startBtn.OnPress += new TitleButton.PressedEventHandler(_startBtn_OnPress);

            _helpBtn = new TitleButton(
                content.Load<Texture2D>("HelpbtnSelected"),
                content.Load<Texture2D>("HelpbtnSelected"),
                select, new Rectangle(100, 570, 256, 128));
            _helpBtn.OnPress += new TitleButton.PressedEventHandler(_helpBtn_OnPress);

            _taBtn = new TitleButton(
                content.Load<Texture2D>("TABtn"),
                content.Load<Texture2D>("TABtn"),
                select, new Rectangle(100, 430, 256, 128));
            _taBtn.OnPress += new TitleButton.PressedEventHandler(_taBtn_OnPress);

            _quitBtn = new TitleButton(
                content.Load<Texture2D>("QuitBtnSelected"),
                content.Load<Texture2D>("QuitBtnSelected"),
                select, new Rectangle(100, 710, 256, 128));
            _quitBtn.OnPress += new TitleButton.PressedEventHandler(_quitBtn_OnPress);

            _muteBtn = new TitleButton(
                content.Load<Texture2D>("MuteBtn"),
                content.Load<Texture2D>("MuteBtn"),
                select, new Rectangle(480-64, 0, 64, 64));
            //TODO add event handler here

            _spriteBatch = new SpriteBatch(graphics);
        }

        void _campaignBtn_OnPress(object sender, EventArgs e)
        {
            //Add cutscene here
            ChangeScreen(_parent.CampaignMap);
        }

        void _helpBtn_OnPress(object sender, EventArgs e)
        {
            ChangeScreen(_parent.HelpScreen);
        }

        void _quitBtn_OnPress(object sender, EventArgs e)
        {
            _parent.Exit();
        }

        void _startBtn_OnPress(object sender, EventArgs e)
        {
            ChangeScreen(_parent.PlayState);
        }

        void _taBtn_OnPress(object sender, EventArgs e)
        {
            ChangeScreen(_parent.TimeAttack);
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            switch (titleState)
            {
                case TitleState.Main:
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        {
                            _parent.Exit();
                        }
                        _campaignBtn.Update(newMouse, oldMouse);
                        _taBtn.Update(newMouse, oldMouse);
                        _startBtn.Update(newMouse, oldMouse);
                        _helpBtn.Update(newMouse, oldMouse);
                        _quitBtn.Update(newMouse, oldMouse);
                        break;
                    }
                case TitleState.Campaign:
                    {
                        break;
                    }
                case TitleState.Help:
                    {

                        break;
                    }
                case TitleState.PlayGame:
                    {
                        break;
                    }
            }
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_titleTex, new Vector2(0,0), Color.White);
            switch (titleState)
            {
                case TitleState.Main:
                    {
                        _taBtn.Draw(_spriteBatch);
                        _campaignBtn.Draw(_spriteBatch);
                        _startBtn.Draw(_spriteBatch);
                        _helpBtn.Draw(_spriteBatch);
                        _quitBtn.Draw(_spriteBatch);
                        _muteBtn.Draw(_spriteBatch);
                        break;
                    }
                case TitleState.Help:
                    {
                        break;
                    }
            }
            base.Draw();
            _spriteBatch.End();
        }
    }
}
