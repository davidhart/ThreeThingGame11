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

        public enum UnitType
        {
            Marine,
            Hydro,
            Launcher,
            Juggernaught,
            Gunship
        }

        UnitType _spawnType;

        public UIBtn(
            Texture2D inBtntex, 
            Texture2D inBtnTexClick,
            SoundEffect inBtnSE,
            int xPos, 
            int yPos,
            Arena arena,
            UnitType type)
        {
            _uiButtonTex = inBtntex;
            _uiButtonTexClick = inBtnTexClick;
            _uiButtonSound = inBtnSE;
            _uiRect = new Rectangle(xPos, yPos, 128, 128);
            _mouseRect = new Rectangle(0, 0, 32, 32);
            _arena = arena;
            _spawnType = type;
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

        public void Update(MouseState mousestate)
        {
            _mouseRect.X = mousestate.X;
            _mouseRect.Y = mousestate.Y;

            if (_mouseRect.Intersects(_uiRect) && mousestate.LeftButton == ButtonState.Pressed)
            {
                _clicked = true;

                switch (_spawnType)
                {
                    case UnitType.Marine:
                        {
                            _arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
                            break;
                        }
                    case UnitType.Hydro:
                        {
                            break;
                        }
                    case UnitType.Launcher:
                        {
                            break;
                        }
                    case UnitType.Juggernaught:
                        {
                            break;
                        }
                    case UnitType.Gunship:
                        {
                            break;
                        }
                }
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
        public void Load(ContentManager content, Arena arena)
        {
            _bgRect = new Rectangle(
                0, 720 - 200, (720 / 2), 200);
            _bgTex = content.Load<Texture2D>("UIBG");
            _marineBtn = new UIBtn(
                content.Load<Texture2D>("MarineSpawnBtn"),
                content.Load<Texture2D>("MarineSpawnBtnClick"),
                content.Load<SoundEffect>("Marine1"),
                128, (720 - 128), arena, UIBtn.UnitType.Marine);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_bgTex, _bgRect, Color.White);
            _marineBtn.Draw(spritebatch);
        }
        public void Update(MouseState mouse)
        {
            _marineBtn.Update(mouse);
        }
    }
}
