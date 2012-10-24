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
        Rectangle _uiRect, _mouseRect;
        Arena _arena;
        UnitEnum _unitEnum;
        int _energyUse;

        public bool isTimeAttack = false;

        Rectangle _sourceRect;
        Rectangle _sourceRectHighlight;
        Texture2D _spriteSheet;


        float _clickAlpha;


        public UIBtn(
            Texture2D spriteSheet,
            Rectangle sourceRect,
            Rectangle sourceRectHighlight,
            Rectangle rectangle,
            Arena arena,
            UnitEnum unitEnum,
            int energyUse)
        {
            _spriteSheet = spriteSheet;
            _sourceRect = sourceRect;
            _sourceRectHighlight = sourceRectHighlight;

            _uiRect = rectangle;
            _mouseRect = new Rectangle(0, 0, 5, 5);
            _arena = arena;
            _unitEnum = unitEnum;
            _energyUse = energyUse;

            _clickAlpha = 0;
        }

        public void Press()
        {
            if (_arena.P1Energy >= _energyUse)
            {
                _clickAlpha = 1;
                _arena.AddUnit(_unitEnum, UnitTeam.Player1);
                _arena.P1Energy -= _energyUse;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (_clickAlpha > 0)
            {
                spritebatch.Draw(_spriteSheet, _uiRect, _sourceRectHighlight, new Color(1.0f, 1.0f, 1.0f, (float)Math.Pow(_clickAlpha, 0.25)));
            }

            spritebatch.Draw(_spriteSheet, _uiRect, _sourceRect, Color.White);
        }

        public void Update(MouseState newMousestate, MouseState oldMouseState, float dt)
        {
            _mouseRect.X = newMousestate.X;
            _mouseRect.Y = newMousestate.Y;

            _clickAlpha -= dt * 3;

            if (_mouseRect.Intersects(_uiRect) &&
                newMousestate.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released &&
                _arena.P1Energy >= _energyUse)
            {
                Press();
            }
        }
    }

    public class UI
    {
        Rectangle _leftAvatarRectangle, _rightAvatarRectangle;
        Texture2D _leftAvatarTexture, _rightAvatarTexture;
        UIBtn _marineBtn, _hydroBtn, _juggernaughtBtn;
        Arena _arena;
        HealthBar _p1HealthBar;
        HealthBar _p2HealthBar;
        KeyboardState _prevKeyState;

        BitmapFont _bmFont;
        FancyText _scoreFancyText;
        ScoreTextColorer _scoreColorer;

        Rectangle _drawPosition;
        Vector2 _scoreDrawPosition;

        Texture2D _UISheet;
        public bool isTimeAtk = false;

        public UI(Rectangle drawPosition)
        {
            _drawPosition = drawPosition;

            _bmFont = new BitmapFont("font", 37, 4);
            _bmFont.SetSpacing(-1);
            _scoreFancyText = new FancyText(_bmFont);
            _scoreColorer = new ScoreTextColorer();
            _scoreFancyText.SetMessage("0", _scoreColorer);

        }

        public void Load(ContentManager content, Arena arena)
        {
            _arena = arena;

            const int avatarVerticalOffset = 115;
            const int healthVarVerticalOffset = avatarVerticalOffset + 7;
            const int healthBarHorizontalOffset = 48;
            const int avatarEdgePadding = 6;
            const int healthBarWidth = 180;
            const int buttonPadding = 8;
            const int buttonVerticalOffset = 10;
            const int buttonSize = 51;
            const int scoreVerticalOffset = 74;

            _scoreDrawPosition = new Vector2((_drawPosition.Left + _drawPosition.Right) / 2, _drawPosition.Top + scoreVerticalOffset);
            _bmFont.LoadContent(content);

            Rectangle buttonRectangle = new Rectangle(_drawPosition.X + buttonPadding, _drawPosition.Y + buttonVerticalOffset, buttonSize, buttonSize);

            _UISheet = content.Load<Texture2D>("UISheet");

            _marineBtn = new UIBtn(
                _UISheet,
                new Rectangle(0, 0, 51, 51),
                new Rectangle(51 * 3, 0, 51, 51),
                buttonRectangle,
                arena,
                UnitEnum.Marine, 
                100);

            buttonRectangle.X += buttonPadding + buttonRectangle.Width;
            _hydroBtn = new UIBtn(
                _UISheet,
                new Rectangle(51, 0, 51, 51),
                new Rectangle(51 * 3, 0, 51, 51),
                buttonRectangle,
                arena,
                UnitEnum.JetpackMarine, 
                200);

            buttonRectangle.X += buttonPadding + buttonRectangle.Width;
            _juggernaughtBtn = new UIBtn(
                _UISheet,
                new Rectangle(51 * 2, 0, 51, 51),
                new Rectangle(51 * 3, 0, 51, 51),
                buttonRectangle,
                arena,
                UnitEnum.Juggernaught,
                800
                );

            _leftAvatarTexture = content.Load<Texture2D>("HumanCommander");
            _leftAvatarRectangle = new Rectangle(
                                        _drawPosition.X + avatarEdgePadding, 
                                        _drawPosition.Y + avatarVerticalOffset, 
                                        _leftAvatarTexture.Width, 
                                        _leftAvatarTexture.Height
                                        );

            _p1HealthBar = new HealthBar(_arena.GetBase1(), 
                                         new Vector2(_drawPosition.X + healthBarHorizontalOffset, _drawPosition.Y + healthVarVerticalOffset), 
                                         healthBarWidth, 
                                         true);

            _p1HealthBar.LoadContent(content);

            _rightAvatarTexture = content.Load<Texture2D>("AlienCommander");
            _rightAvatarRectangle = new Rectangle(
                                         _drawPosition.Right - _rightAvatarTexture.Width - avatarEdgePadding, 
                                         _drawPosition.Y + avatarVerticalOffset, 
                                         _leftAvatarTexture.Width, 
                                         _leftAvatarTexture.Height
                                         );

            _p2HealthBar = new HealthBar(_arena.GetBase2(), 
                                         new Vector2(_drawPosition.Right - healthBarHorizontalOffset - healthBarWidth, _drawPosition.Y + healthVarVerticalOffset), 
                                         healthBarWidth, 
                                         false);

            _p2HealthBar.LoadContent(content);

            _prevKeyState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //spritebatch.Draw(_puzzleBGTex, _puzzleBGRect, Color.White);
            _p1HealthBar.Draw(spritebatch);
            if (!isTimeAtk)
            {
                _p2HealthBar.Draw(spritebatch);
                spritebatch.Draw(_rightAvatarTexture, _rightAvatarRectangle, _arena.GetBase2().GetHitColor());
            }
            spritebatch.Draw(_leftAvatarTexture, _leftAvatarRectangle, _arena.GetBase1().GetHitColor());
            _marineBtn.Draw(spritebatch);
            _hydroBtn.Draw(spritebatch);
            _juggernaughtBtn.Draw(spritebatch);

            spritebatch.End();

            _scoreFancyText.Draw(spritebatch, _scoreDrawPosition - new Vector2(_scoreFancyText.GetSize().X / 2, 0));
        }

        public void Update(float dt, MouseState newMouse, MouseState oldMouse)
        {
            _marineBtn.Update(newMouse, oldMouse, dt);
            _hydroBtn.Update(newMouse, oldMouse, dt);
            _juggernaughtBtn.Update(newMouse, oldMouse, dt);

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
                _juggernaughtBtn.Press();
            }

            _scoreFancyText.ChangeMessage(_arena.P1Energy.ToString());
            _scoreFancyText.Update(dt);

            _prevKeyState = state;
        }

        public void SetBases(Unit p1Base, Unit p2Base)
        {
            _p1HealthBar.SetBase(p1Base);
            _p2HealthBar.SetBase(p2Base);
        }
    }
}
