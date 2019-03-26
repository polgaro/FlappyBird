using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FlappyBird.Entities;

namespace FlappyBird.Screens
{
    class GameScreen : Screen
    {
        private Bird _entityBird;
        private List<Entity> _entityObstacles;

        private TimeSpan _previousRefreshTime;
        private TimeSpan _refreshTime;
        private float _refreshRate = 2000;

        private TimeSpan _previousDifficultyTime;
        private TimeSpan _difficultyTime;
        private float _difficultyRate = 30000;

        private TimeSpan _slowModeTime = TimeSpan.Zero;
        
        private bool _isCheckingCollision = false;

        public GameScreen()
        {
            _previousRefreshTime = TimeSpan.Zero;
            _refreshTime = TimeSpan.FromMilliseconds(_refreshRate);

            _previousDifficultyTime = TimeSpan.Zero;
            _difficultyTime = TimeSpan.FromMilliseconds(_difficultyRate);
        }

        public override void LoadContent()
        {
            _entityBird = new Entities.Bird(Entities.Entity.Type.Bird);

            _entityObstacles = new List<Entity>();
            _entityObstacles.Add(new Entity(Entity.Type.None));
            
            base.LoadContent();
        }

        public override void UnloadContent()
        {

            base.UnloadContent();
        }

        public override void Reset()
        {
            _previousRefreshTime = TimeSpan.Zero;
            _refreshTime = TimeSpan.FromMilliseconds(_refreshRate);

            _previousDifficultyTime = TimeSpan.Zero;
            _difficultyTime = TimeSpan.FromMilliseconds(_difficultyRate);

            _entityBird = new Entities.Bird(Entities.Entity.Type.Bird);

            _entityObstacles.Clear();
            _entityObstacles.Add(new Entities.Entity(Entities.Entity.Type.None));
            
            Statics.GAME_SPEED_DIFFICULTY = 1f;
            Statics.GAME_LEVEL = 1;
            Statics.GAME_SCORE = 0;
            Statics.GAME_NEWHIGHSCORE = false;
            Statics.TIME_ACTUALGAMETIME = TimeSpan.Zero;

            Statics.GAME_BACKGROUND.ResetBackgrounds();
            Statics.GAME_FOREGROUND.ResetBackgrounds();
        }

        public override void Update()
        {
            CheckForInput();

            if (Statics.GAME_STATE == Statics.STATE.Playing)
            {
                // Increase game difficulty
                if (Statics.TIME_ACTUALGAMETIME - _previousDifficultyTime > _difficultyTime)
                {
                    _previousDifficultyTime = Statics.TIME_ACTUALGAMETIME;

                    Statics.GAME_SPEED_DIFFICULTY += 0.1f;
                    Statics.GAME_LEVEL++;

                    foreach (ParallaxBackground layer in Statics.GAME_BACKGROUND.BackgroundLayer_Stack.Values)
                    {
                        layer.MoveSpeed -= Statics.GAME_SPEED_DIFFICULTY;
                    }

                    foreach (ParallaxBackground layer in Statics.GAME_FOREGROUND.ForegroundLayer_Stack.Values)
                    {
                        layer.MoveSpeed -= Statics.GAME_SPEED_DIFFICULTY;
                    }

                    _refreshTime = TimeSpan.FromMilliseconds(_refreshTime.TotalMilliseconds - (Statics.GAME_SPEED_DIFFICULTY * 5f));

                    Console.WriteLine("Refresh time: " + _refreshTime.TotalMilliseconds);
                    Console.WriteLine("Background move speed: " + Statics.GAME_BACKGROUND.BackgroundLayer_Stack["Houses"].MoveSpeed);
                }

                // Add new obstacle
                if (Statics.TIME_ACTUALGAMETIME - _previousRefreshTime > _refreshTime)
                {
                    _previousRefreshTime = Statics.TIME_ACTUALGAMETIME;

                    switch (Statics.GAME_WORLD)
                    {
                        case Statics.WORLD.Pipes:
                            {
                                // Add Pipes
                                _entityObstacles.Add(new Entities.Pipe(Entity.Type.Pipe, Statics.GAME_SPEED_DIFFICULTY));
                                break;
                            }
                        case Statics.WORLD.Bullets:
                            {
                                // Play Sound
                                Statics.MANAGER_SOUND.Play("Bullet\\Explode");

                                // Add Bullet
                                _entityObstacles.Add(new Entities.Bullet(Entity.Type.Bullet, Statics.GAME_SPEED_DIFFICULTY));
                                break;
                            }
                        case Statics.WORLD.Paratroopas:
                            {
                                // Add Paratroopa
                                _entityObstacles.Add(new Entities.Paratroopa(Entity.Type.Paratroopa, Statics.GAME_SPEED_DIFFICULTY));
                                break;
                            }
                    }
                }

                foreach (Entities.Entity obstacle in _entityObstacles.ToList())
                {
                    if (!_isCheckingCollision)
                    {
                        foreach (Rectangle obstacleBound in obstacle.Bounds)
                        {
                            _isCheckingCollision = true;

                            //FOR EACH BIRD
                            {
                                bool isCollision = Statics.COLLISION_USESLOPPY ?
                                    Helpers.Collision.IsSloppyCollision(obstacleBound, _entityBird.Bounds[0]) :
                                    Helpers.Collision.IsPixelCollision(obstacleBound, _entityBird.Bounds[0], obstacle.ColorData, _entityBird.ColorData);

                                if (isCollision)
                                {
                                    _entityBird.IsDead = true;
                                }
                                else if (_entityBird.Position.X > obstacleBound.X + obstacleBound.Width && !obstacle.IsScored)
                                {
                                    //please don't make the sound :D :D :D
                                    //Statics.MANAGER_SOUND.Play("Player\\Score");

                                    obstacle.IsScored = true;
                                    _entityBird.Points++;
                                }
                            }

                            if (_entityBird.IsDead)
                                SetGameState(Statics.STATE.GameOver);

                            Statics.GAME_SCORE = _entityBird.Points;

                            if (obstacle.Position.X <= -obstacle.Width)
                                this._entityObstacles.Remove(obstacle);

                            _isCheckingCollision = false;
                        }
                    }
                }

                if (Statics.GAME_SCORE > Statics.GAME_HIGHSCORE)
                {
                    Statics.GAME_HIGHSCORE = Statics.GAME_SCORE;
                    Statics.GAME_NEWHIGHSCORE = true;
                }

                Statics.DEBUG_ENTITIES = _entityObstacles.Count;
                Statics.DEBUG_PLAYER = _entityBird.Position;

                _entityBird.Update();

                foreach (var obstacle in _entityObstacles)
                {
                    obstacle.Update();
                }
            }

            base.Update();
        }

        public override void Draw()
        {
            _entityBird.Draw();

            foreach (var obstacle in _entityObstacles)
            {
                obstacle.Draw();
            }

            base.Draw();
        }

        private void CheckForInput()
        {
            // Input : Keyboard

            if (Statics.MANAGER_INPUT.IsGamepadPressed(Buttons.Back) || Statics.MANAGER_INPUT.IsKeyPressed(Keys.Escape))
            {
                SetGameState(Statics.STATE.Loading);
                Statics.SCREEN_CURRENT = Statics.MANAGER_SCREEN.Stack["Title"];
                Statics.MANAGER_SCREEN.Stack["Game"].Reset();
            }

            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.F3))
                Statics.GAME_USESLOWMODE = Statics.GAME_USESLOWMODE ? false : true;

            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.F2))
            {
                SetGameState(Statics.STATE.Playing);
                Statics.MANAGER_SCREEN.Stack["Game"].Reset();
            }

            if (Statics.MANAGER_INPUT.IsGamepadPressed(Buttons.Start) || Statics.MANAGER_INPUT.IsRightMouseClicked() || Statics.MANAGER_INPUT.IsKeyPressed(Keys.Enter))
            {
                switch (Statics.GAME_STATE)
                {
                    case Statics.STATE.GameOver:
                        {
                            SetGameState(Statics.STATE.Playing);
                            Statics.MANAGER_SCREEN.Stack["Game"].Reset();
                            break;
                        }
                    case Statics.STATE.Paused:
                        {
                            Statics.GAME_STATE = Statics.STATE.Playing;
                            break;
                        }
                    case Statics.STATE.Playing:
                        {
                            Statics.GAME_STATE = Statics.STATE.Paused;
                            break;
                        }
                }
            }
        }

        private void SetGameState(Statics.STATE newState)
        {
            Statics.GAME_STATE = newState;
        }
    }
}
