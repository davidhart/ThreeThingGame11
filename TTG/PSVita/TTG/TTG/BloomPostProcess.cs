using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TTG
{
    class BloomPostProcess
    {
        RenderTarget2D rt1;
        RenderTarget2D rt2;

        Effect effectBlur;
        Effect effectExtract;
        Effect effectCombine;

        SpriteBatch batch;

        Rectangle screenRect;

        GraphicsDevice device;

        float blurAmount;
        float bloomIntensity;
        float baseIntensity;
        float threshold;

        public float BlurAmount { get { return blurAmount; } set { blurAmount = value; } }
        public float BloomIntensity { get { return bloomIntensity; } set { bloomIntensity = value; } }
        public float BaseIntensity { get { return baseIntensity; } set { baseIntensity = value; } }
        public float Threshold { get { return threshold; } set { threshold = value; if (threshold > 1) threshold = 1; if (threshold < 0) threshold = 0; } }

        enum DrawMode
        {
            Extract,
            Blur,
            Combine
        };

        public BloomPostProcess()
        {
        }

        public void LoadContent(GraphicsDevice device, ContentManager Content, int height, int width)
        {
            batch = new SpriteBatch(device);

            effectBlur = Content.Load<Effect>("effectBlur");
            effectExtract = Content.Load<Effect>("effectExtract");
            effectCombine = Content.Load<Effect>("effectCombine");

            int screenWidth = height;
            int screenHeight = width;

            rt1 = new RenderTarget2D(device, screenWidth, screenHeight, false, device.PresentationParameters.BackBufferFormat, device.PresentationParameters.DepthStencilFormat, device.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
            rt2 = new RenderTarget2D(device, screenWidth, screenHeight, false, device.PresentationParameters.BackBufferFormat, device.PresentationParameters.DepthStencilFormat, device.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);

            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
            this.device = device;

            blurAmount = 1;
            baseIntensity = 1;
            bloomIntensity = 1;
            threshold = 0.5f;
        }

        void DrawPass(RenderTarget2D target, Texture2D sampler0, Texture2D sampler1, Effect effect)
        {
            DrawPass(target, sampler0, sampler1, effect, Vector2.Zero);
        }

        void DrawPass(RenderTarget2D target, Texture2D sampler0, Texture2D sampler1, Effect effect, Vector2 position)
        {
            device.SetRenderTarget(target);
            device.Textures[1] = sampler1;

            batch.Begin(0, BlendState.Opaque, SamplerState.LinearClamp, null, null, effect);

            batch.Draw(sampler0, position, Color.White);

            batch.End();
        }

        public void Draw(Texture2D bloomMap, Texture2D baseMap, Vector2 position)
        {
            Draw(bloomMap, baseMap, DrawMode.Combine, position);
        }

        public void DrawDebugBloom(Texture2D bloomMap, Texture2D baseMap, Vector2 position)
        {
            Draw(bloomMap, baseMap, DrawMode.Extract, position);
        }

        public void DrawDebugBlur(Texture2D bloomMap, Texture2D baseMap, Vector2 position)
        {
            Draw(bloomMap, baseMap, DrawMode.Blur, position);
        }

        void Draw(Texture2D bloomMap, Texture2D baseMap, DrawMode mode, Vector2 position)
        {
            PrepareEffectExtract();
            DrawPass(rt1, bloomMap, null, effectExtract);

            if (mode == DrawMode.Extract)
            {
                DrawPass(null, rt1, null, null, position);
                return;
            }

            PrepareEffectHorizontalBlur();
            DrawPass(rt2, rt1, null, effectBlur);

            PrepareEffectVerticalBlur();
            DrawPass(rt1, rt2, null, effectBlur);

            if (mode == DrawMode.Blur)
            {
                DrawPass(null, rt1, null, null, position);
                return;
            }

            PrepartEffectCombine();
            DrawPass(null, rt1, baseMap, effectCombine, position);
        }

        void PrepareEffectExtract()
        {
            EffectParameterCollection parameters = effectExtract.Parameters;
            parameters["Threshold"].SetValue(threshold);
        }

        void PrepareEffectHorizontalBlur()
        {
            SetBlurEffectParameters(1.0f / screenRect.Width, 0.0f);
        }

        void PrepareEffectVerticalBlur()
        {
            SetBlurEffectParameters(0.0f, 1.0f / screenRect.Height);
        }

        void PrepartEffectCombine()
        {
            EffectParameterCollection parameters = effectCombine.Parameters;
            parameters["BloomIntensity"].SetValue(bloomIntensity);
            parameters["BaseIntensity"].SetValue(baseIntensity);
        }

        void SetBlurEffectParameters(float dx, float dy)
        {
            EffectParameter weightsParameter = effectBlur.Parameters["SampleWeights"];
            EffectParameter offsetsParameter = effectBlur.Parameters["SampleOffsets"];

            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }

        float ComputeGaussian(float n)
        {
            float theta = blurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
