using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TTG
{
    struct CutsceneFrame
    {
        public Texture2D Image;
        public string TextLine1;
        public string TextLine2;
        public string TextLine3;
    }

    public class Cutscene : GameState
    {
        List<CutsceneFrame> Frames = new List<CutsceneFrame>();
        Stopwatch stopwatch = new Stopwatch();
        bool timerStarted = false;
        SpriteBatch _spriteBatch;
        int i = 0;
        GameState nextScreen;
        Texture2D cutsceneBG;
        public Cutscene(Game1 parent)
            : base(parent)
        {
            _parent = parent;

        }

        //Each cutscene is made up of frames with an 
        //image and 3 lines of text
        public void AddFrame(Texture2D image,
            string line1, string line2, string line3)
        {
            CutsceneFrame frame;
            frame.Image = image;
            frame.TextLine1 = line1;
            frame.TextLine2 = line2;
            frame.TextLine3 = line3;
            Frames.Add(frame);
        }

        public void SetNextScreen(GameState nextState)
        {
            nextScreen = nextState;
        }

        public override void Reset()
        {
            stopwatch.Reset();
            timerStarted = false;
            base.Reset(); 
            i = 0;
        }

        public override void Load(ContentManager content, GraphicsDevice graphics)
        {
            _spriteBatch = new SpriteBatch(graphics);
            cutsceneBG = content.Load<Texture2D>("CutsceneBG");
            base.Load(content, graphics);
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(cutsceneBG, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(Frames[i].Image, new Vector2(0, 64), Color.White);
            _spriteBatch.End();
            base.Draw();
        }

        public override void Update(GameTime gameTime, MouseState newMouse, MouseState oldMouse)
        {
            while (i < Frames.Count)
            {
                if (!timerStarted)
                {
                    stopwatch.Start();
                    timerStarted = true;
                }
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    ++i;
                    stopwatch.Reset();
                }
            }
            ChangeScreen(nextScreen);
        }
        
    }
}
