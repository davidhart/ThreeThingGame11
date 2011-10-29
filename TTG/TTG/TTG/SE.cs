using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace TTG.Sound
{
    public class SE
    {
        SoundEffect _sound;
        SoundEffectInstance _soundInstance;

        public SE(SoundEffect sound)
        {
            _sound = sound;
            _soundInstance = _sound.CreateInstance(); 
        }

        public void PlayOneShot()
        {
        }
    }
}
