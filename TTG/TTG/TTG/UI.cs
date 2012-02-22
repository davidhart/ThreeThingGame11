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
        SoundEffect _uiButtonSound, _uiNoSpawnSound;
        Rectangle _uiRect, _mouseRect;
        bool _clicked = false;
        Arena _arena;
        UnitEnum _unitEnum;
        int _energyUse;


        public UIBtn(
            Texture2D inBtntex,
            Texture2D inBtnTexClick,
            SoundEffect inBtnSE,
            int xPos,
            int yPos,
            Arena arena,
            UnitEnum unitEnum,
            int energyUse,
            SoundEffect noSpawn)
        {
            _uiButtonTex = inBtntex;
            _uiButtonTexClick = inBtnTexClick;
            _uiButtonSound = inBtnSE;
            _uiRect = new Rectangle(xPos, yPos, 64, 64);
            _mouseRect = new Rectangle(0, 0, 5, 5);
            _arena = arena;
            _unitEnum = unitEnum;
            _energyUse = energyUse;
            _uiNoSpawnSound = noSpawn;

        }

        public void Press()
        {
            if (_arena.P1Energy >= _energyUse)
            {
                _clicked = true;
                _arena.AddUnit(_unitEnum, UnitTeam.Player1);
                _arena.P1Energy -= _energyUse;
                //_uiButtonSound.Play();
            }
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
                Press();

            }
            else if (_mouseRect.Intersects(_uiRect) &&
            newMousestate.LeftButton == ButtonState.Pressed &&
            oldMouseState.LeftButton == ButtonState.Released &&
            _arena.P1Energy < _energyUse)
            {
                _clicked = true;
                //_uiNoSpawnSound.Play();

            }
            else
            {
                _clicked = false;
            }
        }
    }

    public class UI
    {
        Rectangle _hCommanderRect, _aCommanderRect, _puzzleBGRect;
        Texture2D _hCommandertex, _aCommandertex, _puzzleBGTex;
        UIBtn _marineBtn, _hydroBtn, _launcherBtn, _juggernaughtBtn, _gunshipBtn;
        Arena _arena;
        SpriteFont _font;
        HealthBar _p1HealthBar;
        HealthBar _p2HealthBar;
        KeyboardState _prevKeyState;

        public void Load(ContentManager content, Arena arena)
        {
            _font = content.Load<SpriteFont>("UIFont");
            _marineBtn = new UIBtn(
                content.Load<Texture2D>("MarineSpawnBtn"),
                content.Load<Texture2D>("MarineSpawnBtnClick"),
                content.Load<SoundEffect>("Talk1"), 
                12, 50,
                arena,
                UnitEnum.Marine, 100,
                content.Load<SoundEffect>("NoSpawn"));

            _hydroBtn = new UIBtn(
                content.Load<Texture2D>("HydroSpawnBtn"),
                content.Load<Texture2D>("HydroSpawnBtnClick"),
                content.Load<SoundEffect>("HydroTalk1"),
                12, 150,
                arena,
                UnitEnum.Hydro, 150,
                content.Load<SoundEffect>("NoSpawn"));

            _launcherBtn = new UIBtn(
                content.Load<Texture2D>("LauncherSpawnBtn"),
                content.Load<Texture2D>("LauncherSpawnBtnClick"),
                content.Load<SoundEffect>("JuggTalk1"),
                12, 250,
                arena, //Change this
                UnitEnum.Juggernaught,
                1200,
                content.Load<SoundEffect>("NoSpawn")
                );

            _hCommandertex = content.Load<Texture2D>("HumanCommander");
            _hCommanderRect = new Rectangle(6, 600, _hCommandertex.Width, _hCommandertex.Height);
            _aCommandertex = content.Load<Texture2D>("AlienCommander");
            _aCommanderRect = new Rectangle(480 - (_aCommandertex.Width + 10), 600, _hCommandertex.Width, _hCommandertex.Height);
            _arena = arena;
            _puzzleBGTex = content.Load<Texture2D>("PuzzleBG");
            _puzzleBGRect = new Rectangle(0, 0, _puzzleBGTex.Width, _puzzleBGTex.Height);

            _p1HealthBar = new HealthBar(_arena.GetBase1(), new Vector2(48, 600), 150, true);
            _p1HealthBar.LoadContent(content);

            _p2HealthBar = new HealthBar(_arena.GetBase2(), new Vector2(480 - 48 - 150, 600), 150, false);
            _p2HealthBar.LoadContent(content);

            _prevKeyState = Keyboard.GetState();
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_puzzleBGTex, _puzzleBGRect, Color.White);
            spritebatch.DrawString(_font, "ENERGY:" + _arena.P1Energy, new Vector2(550, 537), Color.White);
            _marineBtn.Draw(spritebatch);
            spritebatch.DrawString(_font, "MARINE: 100", new Vector2(76, 50), Color.White);
            _hydroBtn.Draw(spritebatch);
            spritebatch.DrawString(_font, "HYDRO: 150", new Vector2(76, 150), Color.White);
            _launcherBtn.Draw(spritebatch);
            spritebatch.DrawString(_font, "JUGGERNAUGHT: 1200", new Vector2(76, 250), Color.White);
            _p1HealthBar.Draw(spritebatch);
            _p2HealthBar.Draw(spritebatch);
            spritebatch.Draw(_hCommandertex, _hCommanderRect, _arena.GetBase1().GetHitColor());
            spritebatch.Draw(_aCommandertex, _aCommanderRect, _arena.GetBase2().GetHitColor());
        }
        public void Update(MouseState newMouse, MouseState oldMouse)
        {
            _marineBtn.Update(newMouse, oldMouse);
            _hydroBtn.Update(newMouse, oldMouse);
            _launcherBtn.Update(newMouse, oldMouse);

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D1) && _prevKeyState.IsKeyUp(Keys.D1))
            {
                _marineBtn.Press();
            }

            if (state.IsKeyDown(Keys.D2) && _prevKeyState.IsKeyUp(Keys.D2))
            {
                _hydroBtn.Press();
            }

            if (state.IsKeyDown(Keys.D3) && _prevKeyState.IsKeyUp(Keys.D3))
            {
                _launcherBtn.Press();
            }



            _prevKeyState = state;
        }

        public void SetBases(Base p1Base, Base p2Base)
        {
            _p1HealthBar.SetBase(p1Base);
            _p2HealthBar.SetBase(p2Base);
        }
    }
}
