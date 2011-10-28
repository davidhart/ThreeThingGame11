using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class Animation
    {
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }
        private Texture2D _texture;

        public float FrameTime
        {
            get
            {
                return _frameTime;
            }
        }
        private float _frameTime;

        public bool IsLooping
        {
            get
            {
                return _isLooping;
            }
        }
        private bool _isLooping;

        public int NumFrames
        {
            get
            {
                return _numFrames;
            }
        }
        private int _numFrames;

        private int _numFramesWide;
        private int _numFramesTall;
        private int _startIndex;

        public Animation(Texture2D texture, int numFramesWide, int numFramesTall, int startIndex, int numFrames, float frameTime, bool isLooping)
        {
            _texture = texture;
            _frameTime = frameTime;
            _isLooping = isLooping;
            _numFramesWide = numFramesWide;
            _numFramesTall = numFramesTall;
            _startIndex = startIndex;
            _numFrames = numFrames;
        }

        public Rectangle GetFrameRect(int frameID)
        {
            // Frame indices start at the top row incrementing left to right, top to bottom
            int index = frameID + _startIndex;

            int width = Texture.Width / _numFramesWide;
            int height = Texture.Height / _numFramesTall;

            int x = (index % _numFramesWide) * width;
            int y = (index / _numFramesWide) * height;

            return new Rectangle(x, y, width, height);
        }
    }

    public class AnimationPlayer
    {
        private Animation _animation;
        private int _currentFrame;
        private double _elapsedTime;

        public void PlayAnimation(Animation animation)
        {
            if (_animation != animation)
            {
                _animation = animation;
                _currentFrame = 0;
                _elapsedTime = 0.0f;
            }
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            while (_elapsedTime > _animation.FrameTime)
            {
                // Advance the frame index; looping or clamping as appropriate.
                _elapsedTime -= _animation.FrameTime;

                if (_animation.IsLooping)
                {
                    _currentFrame = (_currentFrame + 1) % _animation.NumFrames;
                }
                else
                {
                    _currentFrame = Math.Min(_currentFrame + 1, _animation.NumFrames - 1);
                }
            }
        }

        public Rectangle GetCurrentFrameRectangle()
        {
            return _animation.GetFrameRect(_currentFrame);
        }

        public Texture2D GetCurrentFrameTexture()
        {
            return _animation.Texture;
        }
    }
}
