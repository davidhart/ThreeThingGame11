using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TTG
{
    public class WorldMap : GameState
    {
        Vector2[] playerPos = new Vector2[8];
        Texture2D 
            worlMapTex, 
            playerIconTex,
            EngageBtnTex,
            MoveFwdBtnTex,
            MoveBkBtnTex,
            QuitBtnTex;
        TitleButton EngageBtn, FwdBtn, BkBtn, QuitBtn;
        SpriteFont _font;
        SpriteBatch _spriteBatch;
        int levelNumber = 0;


        public WorldMap(Game1 parent)
            :base(parent)
        {
            playerPos[0] = new Vector2(338, 522);
            playerPos[1] = new Vector2(357, 472);
            playerPos[2] = new Vector2(271, 454);
            playerPos[3] = new Vector2(117, 408);
            playerPos[4] = new Vector2(109, 256);
            playerPos[5] = new Vector2(240, 251);
            playerPos[6] = new Vector2(212, 182);
            playerPos[7] = new Vector2(382, 120);
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            base.Load(content, graphics);

            worlMapTex = content.Load<Texture2D>("volcanis map");
            playerIconTex = content.Load<Texture2D>("player icon");
            MoveBkBtnTex = content.Load<Texture2D>("MoveBk");
            MoveFwdBtnTex = content.Load<Texture2D>("MoveFWD");
            EngageBtnTex = content.Load<Texture2D>("EngageBtn");
            QuitBtnTex = content.Load<Texture2D>("QuitBtnMap");

            _spriteBatch = new SpriteBatch(graphics);

            BkBtn = new TitleButton(
                MoveBkBtnTex, MoveBkBtnTex,
                null,
                new Rectangle(0, 800 - 51, 51, 51));
            BkBtn.OnPress += new TitleButton.PressedEventHandler(bkBtn_OnPress);
           
            FwdBtn = new TitleButton(
                MoveFwdBtnTex, MoveFwdBtnTex,
                null,
                new Rectangle(100, 800 - 51, 51, 51));
            FwdBtn.OnPress += new TitleButton.PressedEventHandler(fwdBtn_OnPress);

            EngageBtn = new TitleButton(
                EngageBtnTex, EngageBtnTex,
                null,
                new Rectangle(200 + 5, 800 - 51, 51, 51));
            EngageBtn.OnPress += new TitleButton.PressedEventHandler(engageBtn_OnPress);

            QuitBtn = new TitleButton(
                QuitBtnTex, QuitBtnTex,
                null,
                new Rectangle(480 - 51, 800 - 51, 51, 51));
            QuitBtn.OnPress += new TitleButton.PressedEventHandler(quitBtn_OnPress);

            _font = content.Load<SpriteFont>("UIFont");
        }

        void bkBtn_OnPress(object sender, EventArgs e)
        {
            if (levelNumber > 0)
            {
                levelNumber--;
            }
        }

        void fwdBtn_OnPress(object sender, EventArgs e)
        {
            if (levelNumber < 7)
            {
                levelNumber++;
            }
        }

        void engageBtn_OnPress(object sender, EventArgs e)
        {
            
        }

        void quitBtn_OnPress(object sender, EventArgs e)
        {
            ChangeScreen(_parent.TitleScreenState);
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(worlMapTex, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(playerIconTex, playerPos[levelNumber], Color.White);
            BkBtn.Draw(_spriteBatch);
            FwdBtn.Draw(_spriteBatch);
            QuitBtn.Draw(_spriteBatch);
            EngageBtn.Draw(_spriteBatch);
            if (levelNumber == 0)
            {
                //fancyText.SetMessage("Goober", colorer);
                _spriteBatch.DrawString(_font, "Goober", new Vector2(20, 650), Color.White);
            }
            if (levelNumber == 1)
            {
                //fancyText.SetMessage("Blortho", colorer)
                _spriteBatch.DrawString(_font, "Blortho", new Vector2(20, 650), Color.White); ;
            }
            if (levelNumber == 2)
            {
                //fancyText.SetMessage("Cumus", colorer);
                _spriteBatch.DrawString(_font, "Cumus", new Vector2(20, 650), Color.White);
            }
            if (levelNumber == 3)
            {
                //fancyText.SetMessage("Rojin", colorer);
                _spriteBatch.DrawString(_font, "Rojin", new Vector2(20, 650), Color.White);
            }
            if (levelNumber == 4)
            {
                //fancyText.SetMessage("Atra", colorer);
                _spriteBatch.DrawString(_font, "Atra", new Vector2(20, 650), Color.White);
            }
            if (levelNumber == 5)
            {
                //fancyText.SetMessage("Senshi", colorer);
                _spriteBatch.DrawString(_font, "Senshi", new Vector2(20, 650), Color.White);
            }
            if (levelNumber == 6)
            {
                //fancyText.SetMessage("Obour", colorer);
                _spriteBatch.DrawString(_font, "Obour", new Vector2(20, 650), Color.White);
            }
            if (levelNumber == 7)
            {
                _spriteBatch.DrawString(_font, "Spardi", new Vector2(20, 650), Color.White);
            }
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            BkBtn.Update(newMouse, oldMouse);
            FwdBtn.Update(newMouse, oldMouse);
            EngageBtn.Update(newMouse, oldMouse);
            QuitBtn.Update(newMouse, oldMouse);
            
            //base.Update(gameTime, newMouse, oldMouse);
        }
    }
}
