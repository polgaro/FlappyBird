using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlappyBird.Managers
{
    class TextureManager
    {
        public Dictionary<string, Texture2D> Textures;
        public Dictionary<string, Texture2D> AnimatedTextures;

        public TextureManager()
        {
            Statics.MANAGER_TEXTURES = this;

            Textures = new Dictionary<string, Texture2D>();
            AnimatedTextures = new Dictionary<string, Texture2D>();
        }

        public void LoadContent()
        {
            #region Entity Textures

            AnimatedTextures.Add("Entity\\Bird", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_bird_animated.png"));
            AnimatedTextures.Add("Entity\\Paratroopa", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_paratroopa_animated.png"));

            Textures.Add("Entity\\Bird", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_bird.png"));
            Textures.Add("Entity\\DeadBird", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_bird_dead.png"));
            Textures.Add("Entity\\Boomba", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_boomba.png"));
            Textures.Add("Entity\\Bullet", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_bullet.png"));
            Textures.Add("Entity\\Paratroopa", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_paratroopa.png"));
            Textures.Add("Entity\\Pipe", ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_pipe.png"));

            #endregion

            #region UI Textures

            Textures.Add("UI\\Button", ParallaxBackground.GetFromPng("Content\\Textures\\Button\\flappy_button"));
            Textures.Add("UI\\ButtonExit", ParallaxBackground.GetFromPng("Content\\Textures\\Button\\flappy_button_exit"));
            Textures.Add("UI\\ButtonRestart", ParallaxBackground.GetFromPng("Content\\Textures\\Button\\flappy_button_restart"));
            Textures.Add("UI\\ButtonPipe", ParallaxBackground.GetFromPng("Content\\Textures\\Button\\flappy_level_pipes"));
            Textures.Add("UI\\ButtonBullet", ParallaxBackground.GetFromPng("Content\\Textures\\Button\\flappy_level_bullet"));
            Textures.Add("UI\\ButtonParatroopa", ParallaxBackground.GetFromPng("Content\\Textures\\Button\\flappy_level_paratroopa"));
            
            #endregion
        }
    }
}
