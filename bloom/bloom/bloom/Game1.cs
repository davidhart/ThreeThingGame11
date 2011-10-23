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

namespace Effects
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RenderTarget2D rtGather;

        SpriteFont font;
        Texture2D test;

        BloomPostProcess bloomPP;
        int debugMode = 0;

        KeyboardState prevKeyState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            bloomPP = new BloomPostProcess();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");
            test = Content.Load<Texture2D>("metalslug02");
            graphics.PreferredBackBufferWidth = test.Width * 3;
            graphics.PreferredBackBufferHeight = test.Height*3;
            graphics.ApplyChanges();

            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            rtGather = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                                   RenderTargetUsage.DiscardContents);
            bloomPP.LoadContent(GraphicsDevice, Content, pp);

            prevKeyState = Keyboard.GetState();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.M) && prevKeyState.IsKeyUp(Keys.M))
            {
                debugMode++;

                if (debugMode > 2)
                    debugMode = 0;
            }

            if (keyState.IsKeyDown(Keys.OemPlus))
                bloomPP.BlurAmount = bloomPP.BlurAmount + 0.03f;

            if (keyState.IsKeyDown(Keys.OemMinus))
                bloomPP.BlurAmount = bloomPP.BlurAmount - 0.03f;

            if (keyState.IsKeyDown(Keys.P))
                bloomPP.BaseIntensity = bloomPP.BaseIntensity + 0.03f;

            if (keyState.IsKeyDown(Keys.O))
                bloomPP.BaseIntensity = bloomPP.BaseIntensity - 0.03f;

            if (keyState.IsKeyDown(Keys.L))
                bloomPP.BloomIntensity = bloomPP.BloomIntensity + 0.03f;

            if (keyState.IsKeyDown(Keys.K))
                bloomPP.BloomIntensity = bloomPP.BloomIntensity - 0.03f;

            if (keyState.IsKeyDown(Keys.OemPeriod))
                bloomPP.Threshold = bloomPP.Threshold + 0.03f;

            if (keyState.IsKeyDown(Keys.OemComma))
                bloomPP.Threshold = bloomPP.Threshold - 0.03f;

            base.Update(gameTime);

            prevKeyState = keyState;
        }

        void DrawFullscreenQuad(RenderTarget2D target, Texture2D texture, Effect effect)
        {
            GraphicsDevice.SetRenderTarget(target);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            spriteBatch.Draw(texture, new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight), Color.White);
            spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(rtGather);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            spriteBatch.Draw(test, new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight), Color.White);
            spriteBatch.End();

            string s = "Mode: ";

            if (debugMode == 0)
            {
                bloomPP.Draw(rtGather, rtGather);
                s += "Default";
            }
            else if (debugMode == 1)
            {
                bloomPP.DrawDebugBloom(rtGather, rtGather);
                s += "Bloom Map";
            }
            else
            {
                bloomPP.DrawDebugBlur(rtGather, rtGather);
                s += "Blur";
            }

            spriteBatch.Begin();

            s += "\nBlurAmount: " + bloomPP.BlurAmount + "\nBloomIntensity: " + bloomPP.BloomIntensity + "\nBaseIntensity: " + bloomPP.BaseIntensity +
                    "\nThreshold: " + bloomPP.Threshold;

            spriteBatch.DrawString(font, s, Vector2.Zero, Color.Magenta);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
