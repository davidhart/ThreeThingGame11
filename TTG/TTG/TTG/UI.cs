using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace TTG
{
    public class UIBtn
    {
        Texture2D _uiButtonTex, _uiButtonTexClick;
        SoundEffect _uiButtonSound;
        Rectangle _uiRect, _mouseRect;
        bool _clicked = false;
        Arena _arena;
        UnitEnum _unitEnum;

        public UIBtn(
            Texture2D inBtntex,
            Texture2D inBtnTexClick,
            SoundEffect inBtnSE,
            int xPos,
            int yPos,
            Arena arena,
            UnitEnum unitEnum)
        {
            _uiButtonTex = inBtntex;
            _uiButtonTexClick = inBtnTexClick;
            _uiButtonSound = inBtnSE;
            _uiRect = new Rectangle(xPos, yPos, 64, 64);
            _mouseRect = new Rectangle(0, 0, 5, 5);
            _arena = arena;
            _unitEnum = unitEnum;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (_clicked)
            {
                spritebatch.Draw(_uiButtonTexClick, _uiRect, Color.White);
            }
            else
            {
                spritebatch.Draw(_uiButtonTex, _uiRect, Color.White);
            }
        }

        public void Update(MouseState newMousestate, MouseState oldMouseState)
        {
            _mouseRect.X = newMousestate.X;
            _mouseRect.Y = newMousestate.Y;

            if (_mouseRect.Intersects(_uiRect) &&
                newMousestate.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released)
            {
                _clicked = true;
                _arena.AddUnit(_unitEnum, UnitTeam.Player1);
                _uiButtonSound.Play();

            }
            else
            {
                _clicked = false;
            }
        }
    }

    public class UI
    {
        Rectangle _bgRect;
        Texture2D _bgTex;
        UIBtn _marineBtn, _hydroBtn, _launcherBtn, _juggernaughtBtn, _gunshipBtn;
        Arena _arena;
        SpriteFont _font;
        public void Load(ContentManager content, Arena arena)
        {
            _font = content.Load<SpriteFont>("UIFont");

            _bgRect = new Rectangle(
                0, 720 - 100, (720 / 2), 100);
            _bgTex = content.Load<Texture2D>("UIBG");
            _marineBtn = new UIBtn(
                content.Load<Texture2D>("MarineSpawnBtn"),
                content.Load<Texture2D>("MarineSpawnBtnClick"),
                content.Load<SoundEffect>("Talk1"), 
                12, (720 - 80),
                arena,
                UnitEnum.Marine);

            _hydroBtn = new UIBtn(
                content.Load<Texture2D>("HydroSpawnBtn"),
                content.Load<Texture2D>("HydroSpawnBtnClick"),
                content.Load<SoundEffect>("HydroTalk1"),
                80, 640,
                arena,
                UnitEnum.Marine);//Change this

            _launcherBtn = new UIBtn(
            content.Load<Texture2D>("LauncherSpawnBtn"),
            content.Load<Texture2D>("LauncherSpawnBtnClick"),
            content.Load<SoundEffect>("LauncherTalk1"),
            148, 640,
            arena,
            UnitEnum.Marine);//Change this

            _arena = arena;

        }
        public void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(_bgTex, _bgRect, Color.White);
            spritebatch.DrawString(_font, "ENERGY:", new Vector2(12, 600), Color.White);
            _marineBtn.Draw(spritebatch);
            _hydroBtn.Draw(spritebatch);
            _launcherBtn.Draw(spritebatch);
            
        }
        public void Update(MouseState newMouse, MouseState oldMouse)
        {
            _marineBtn.Update(newMouse, oldMouse);
            _hydroBtn.Update(newMouse, oldMouse);
            _launcherBtn.Update(newMouse, oldMouse);
        }
    }
}
