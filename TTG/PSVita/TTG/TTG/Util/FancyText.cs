﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    interface FancyTextColorer
    {
        float Scale(float elapsed);
        bool Draw(float elapsed);
        Color BackgroundColor(float elapsed);
        Color UpperColor(float elapsed);
        Color LowerColor(float elapsed);
    }

    class HighlightTextColorer : FancyTextColorer
    {
        Vector3 color;

        public HighlightTextColorer(Color color)
        {
            this.color = color.ToVector3();
        }

        public float Scale(float elapsed)
        {
            return 5 + (float)Math.Pow(elapsed *2.0f, 3);
        }

        public bool Draw(float elapsed)
        {
            return elapsed < 1.0f;
        }

        protected float Alpha(float elapsed)
        {
            return Util.Clamp((float)(1.0f - Math.Pow((float)Math.Max(0.0f, -1.6f + elapsed * 3.5f), 2.0f)), 0.0f, 1.0f);
        }

        public Color BackgroundColor(float elapsed)
        {
            float a = Alpha(elapsed);
            return new Color(1.0f, 1.0f, 1.0f, a);
        }

        public Color UpperColor(float elapsed)
        {
            float t = (float)Math.Pow(Math.Max(0, Math.Abs(1.5f - elapsed * 8)), 3);
            Vector3 c = Util.Lerp(new Vector3(1), color, t);
            float a = Alpha(elapsed);
            return new Color(c.X, c.Y, c.Z, a);
        }

        public Color LowerColor(float elapsed)
        {
            float t = (float)Math.Pow(Math.Max(0, Math.Abs(1 - elapsed * 8)), 3);
            Vector3 c = Util.Lerp(new Vector3(1), color, t);
            float a = Alpha(elapsed);
            return new Color(c.X, c.Y, c.Z, a);
        }
    }

    class RainbowTextColorer : FancyTextColorer
    {
        public float Scale(float elapsed)
        {
            return 5 + (float)Math.Pow(elapsed * 2.0f, 3);
        }

        public bool Draw(float elapsed)
        {
            return elapsed < 1.0f;
        }

        protected float Alpha(float elapsed)
        {
            return Util.Clamp((float)(1.0f - Math.Pow((float)Math.Max(0.0f, -1.6f + elapsed * 3.5f), 2.0f)), 0.0f, 1.0f);
        }

        public Color BackgroundColor(float elapsed)
        {
            float a = Alpha(elapsed);
            return new Color(1.0f, 1.0f, 1.0f, a);
        }

        public Color UpperColor(float elapsed)
        {
            Color c = Util.ColorFromHSV((int)(elapsed * 800) % 360, 255, 255);
            c.A = (byte)(Alpha(elapsed) * 255.0f);
            return c;
        }

        public Color LowerColor(float elapsed)
        {
            Color c = Util.ColorFromHSV((int)(elapsed * 800 + 60) % 360, 255, 255);
            c.A = (byte)(Alpha(elapsed) * 255.0f);
            return c;
        }
    }

    class ScoreTextColorer : FancyTextColorer
    {
        public float Scale(float elapsed)
        {
            return 3;
        }

        public bool Draw(float elapsed)
        {
            return true;
        }

        protected float Alpha(float elapsed)
        {
            return Util.Clamp((float)(1.0f - Math.Pow((float)Math.Max(0.0f, -1.6f + elapsed * 3.5f), 2.0f)), 0.0f, 1.0f);
        }

        public Color BackgroundColor(float elapsed)
        {
            return new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public Color UpperColor(float elapsed)
        {
            Color c = new Color(255, 255, 255, 255);
            return c;
        }

        public Color LowerColor(float elapsed)
        {
            Color c = new Color(255, 255, 255, 255);
            return c;
        }
    }

    class FancyText
    {
        BitmapFont font;
        FancyTextColorer colorer;
        string text;
        float time;

        public FancyText(BitmapFont font)
        {
            this.font = font;
            time = 0;
        }

        public void SetMessage(string text, FancyTextColorer colorer)
        {
            this.text = text;
            this.colorer = colorer;
            time = 0;
        }

        public void ChangeMessage(string text)
        {
            this.text = text;
        }

        public void Update(float dt)
        {
            time += dt;
        }

        public Vector2 GetSize()
        {
            if (colorer == null)
                return Vector2.Zero;

            float scale = colorer.Scale(time);
            return new Vector2(text.Length * font.GetCharSize().X *scale, font.GetCharSize().Y * scale);
        }

        public bool IsVisible()
        {
            if (colorer == null)
                return false;

            return colorer.Draw(time);
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            if (text == null)
                return;

            if (text.Length == 0)
                return;

            if (colorer.Draw(time))
            {
                float scale = colorer.Scale(time);
                batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                font.DrawText(batch, text, position, colorer.BackgroundColor(time), scale, 0);
                font.DrawText(batch, text, position, colorer.UpperColor(time), scale, 37 * 2);
                font.DrawText(batch, text, position, colorer.LowerColor(time), scale, 37);
                batch.End();
            }
        }
    }
}
