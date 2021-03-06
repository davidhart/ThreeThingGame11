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
        Help,
        PlayGame
    }
    public class TitleButton
    {
        Texture2D _uiButtonTex, _uiButtonTexClick;
        SoundEffect _uiButtonSound;
        Rectangle _buttonRect, _mouseRect;

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
                _uiButtonSound.Play();

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
    public class TitleScreen : GameScreen
    {
        TitleState titleState = TitleState.Main;
        Texture2D _titleTex;
        Rectangle _titleRect;
        TitleButton _startBtn, _helpBtn, _quitBtn;

        public override void Load(ContentManager content)
        {
            _titleRect = new Rectangle(0, 0, 1280, 768);
            _titleTex = content.Load<Texture2D>("TitleBest");

            SoundEffect select = content.Load<SoundEffect>("Select");

            _startBtn = new TitleButton(
                content.Load<Texture2D>("StartBtn"),
                content.Load<Texture2D>("StartBtnSelected"),
                select, new Rectangle (640 - 128, 300, 256, 128));

            _helpBtn = new TitleButton(
                content.Load<Texture2D>("Helpbtn"),
                content.Load<Texture2D>("HelpbtnSelected"),
                select, new Rectangle(640 - 128, 400, 256, 128));

            _quitBtn = new TitleButton(
                content.Load<Texture2D>("QuitBtn"),
                content.Load<Texture2D>("QuitBtnSelected"),
                select, new Rectangle(640 - 128, 500, 256, 128));

            base.Load(content);
        }

        public override void Update(MouseState newMouse, MouseState oldMouse)
        {
            switch (titleState)
            {
                case TitleState.Main:
                    {
                        _startBtn.Update(newMouse, oldMouse);
                        _helpBtn.Update(newMouse, oldMouse);
                        _quitBtn.Update(newMouse, oldMouse);
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

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(_titleTex, _titleRect, Color.White);
            switch (titleState)
            {
                case TitleState.Main:
                    {
                        _startBtn.Draw(spritebatch);
                        _helpBtn.Draw(spritebatch);
                        _quitBtn.Draw(spritebatch);
                        break;
                    }
                case TitleState.Help:
                    {
                        break;
                    }
            }
            base.Draw(spritebatch);
            spritebatch.End();
        }
    }
}
