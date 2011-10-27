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


namespace TTG
{
    public class Music
    {
        bool _isPlaying;
        SoundEffect _tune;
        SoundEffectInstance _music;

        public Music(SoundEffect track, bool looped)
        {
            _tune = track;
            _music = _tune.CreateInstance();
            _music.IsLooped = looped;
            
        }
        public void Play()
        {
            if (_music.State == SoundState.Stopped)
            {
                _music.Play();
            }
        }
        public void Stop()
        {
            _music.Stop();
        }
    }
}
