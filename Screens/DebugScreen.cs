using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FlappyBird.Screens
{
    class DebugScreen : Screen
    {
        private Helpers.FPSMonitor _fpsMonitor;
        
        private Vector2 _fpsPosition;
        private Vector2 _entityCountPosition;
        private Vector2 _runningTimePosition;
        private Vector2 _gameStatePosition;
        private Vector2 _playerPosition;
        private Vector2 _slowmodePosition;

        public DebugScreen()
        {
            _fpsMonitor = new Helpers.FPSMonitor();
        }

        public override void LoadContent()
        {
            Statics.TEXTURE_PIXEL = ParallaxBackground.GetFromPng("Content\\Textures\\flappy_pixel");

            base.LoadContent();
        }

        public override void Update()
        {
            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.F4))
            {
                Statics.DEBUG_SHOWTEXT = Statics.DEBUG_SHOWTEXT ? false : true;
            }

            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.F5))
            {
                Statics.DEBUG_SHOWHITBOX = Statics.DEBUG_SHOWHITBOX ? false : true;
            }

            if (Statics.DEBUG_SHOWTEXT)
            {
                // Check the current FPS of game
                _fpsMonitor.Update();

                // Update text positions
                _fpsPosition = new Vector2(Statics.GAME_WIDTH - Statics.MANAGER_FONT.Library["Small"].MeasureString(String.Format("FPS: {0}", Statics.DEBUG_FPS.ToString("00"))).X - 20, 20);
                _entityCountPosition = new Vector2(Statics.GAME_WIDTH - Statics.MANAGER_FONT.Library["Small"].MeasureString(String.Format("Entities: {0}", Statics.DEBUG_ENTITIES.ToString("00"))).X - 20, 50);
                _runningTimePosition = new Vector2(Statics.GAME_WIDTH - Statics.MANAGER_FONT.Library["Small"].MeasureString(String.Format("Time: {0} seconds", Statics.TIME_ACTUALGAMETIME.TotalSeconds.ToString("00"))).X - 20, 80);
                _gameStatePosition = new Vector2(Statics.GAME_WIDTH - Statics.MANAGER_FONT.Library["Small"].MeasureString(String.Format("State: {0}", Statics.GAME_STATE)).X - 20, 140);
                _playerPosition = new Vector2(Statics.GAME_WIDTH - Statics.MANAGER_FONT.Library["Small"].MeasureString(String.Format("Player X:{0} Y:{1}", Statics.DEBUG_PLAYER.X.ToString("000"), Statics.DEBUG_PLAYER.Y.ToString("000"))).X - 20, 170);
                _slowmodePosition = new Vector2(Statics.GAME_WIDTH - Statics.MANAGER_FONT.Library["Small"].MeasureString(String.Format("Slow Mode: {0}", Statics.GAME_USESLOWMODE)).X - 20, 200);
            }

            base.Update();
        }

        public override void Draw()
        {
            if (Statics.DEBUG_SHOWTEXT)
            {
                Helpers.Debug.DrawText(_fpsPosition, String.Format("FPS: {0}", Statics.DEBUG_FPS.ToString("00")));
                Helpers.Debug.DrawText(_entityCountPosition, String.Format("Entities: {0}", Statics.DEBUG_ENTITIES.ToString("00")));
                Helpers.Debug.DrawText(_runningTimePosition, String.Format("Time: {0} seconds", Statics.TIME_ACTUALGAMETIME.TotalSeconds.ToString("00")));
                Helpers.Debug.DrawText(_gameStatePosition, String.Format("State: {0}", Statics.GAME_STATE));
                Helpers.Debug.DrawText(_playerPosition, String.Format("Player X:{0} Y:{1}", Statics.DEBUG_PLAYER.X.ToString("000"), Statics.DEBUG_PLAYER.Y.ToString("000")));
                Helpers.Debug.DrawText(_slowmodePosition, String.Format("Slow Mode: {0}", Statics.GAME_USESLOWMODE));
            }

            base.Draw();
        }
    }
}
