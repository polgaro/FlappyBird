using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;

namespace FlappyBird.Managers
{
    public class SoundManager
    {
        private AudioEngine _engine;
        private SoundBank _soundBank;
        private WaveBank _waveBank;

        public SoundManager()
        {
            Statics.MANAGER_SOUND = this;
        }

        public void LoadContent()
        {
            _engine = new AudioEngine("Content\\Sounds\\flappy_audio.xgs");
            _soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
            _waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");
        }

        public void Play(string sound)
        {
            try
            {
                _soundBank.PlayCue(sound);
            }
            catch { }
        }
    }
}
