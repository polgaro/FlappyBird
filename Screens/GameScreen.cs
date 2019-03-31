using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FlappyBird.Entities;
using FlappyBird.AI;
using System.Xml;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeatLib.Model;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace FlappyBird.Screens
{
    public class GameScreen : Screen
    {
        NeatEvolutionAlgorithm<NeatGenome> _ea;

        private List<Bird> birds = new List<Bird> { }; //_entityBird;
        private static List<Entity> _entityObstacles;

        private TimeSpan _previousRefreshTime;
        private TimeSpan _refreshTime;
        private float _refreshRate = 2000;

        private TimeSpan _previousDifficultyTime;
        private TimeSpan _difficultyTime;
        private float _difficultyRate = 30000;

        private TimeSpan _slowModeTime = TimeSpan.Zero;
        
        private bool _isCheckingCollision = false;
        private FlappyBirdExperiment experiment;
        private CreateOffpringDTO<NeatGenome> offspringData;

        public static List<Pipe> PipeObstacles
        {
            get
            {
                return _entityObstacles.OfType<Pipe>().ToList();
            }
        }

        public GameScreen()
        {
            _previousRefreshTime = TimeSpan.Zero;
            _refreshTime = TimeSpan.FromMilliseconds(_refreshRate);

            _previousDifficultyTime = TimeSpan.Zero;
            _difficultyTime = TimeSpan.FromMilliseconds(_difficultyRate);
        }

        public override void LoadContent()
        {
            experiment = new FlappyBirdExperiment();

            // Load config XML.
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("expconfig.xml");
            experiment.Initialize("TicTacToe", xmlConfig.DocumentElement);

            // Create evolution algorithm and attach update event.
            _ea = experiment.CreateEvolutionAlgorithm();
            _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);

            InitializeBirds(birds);

            _entityObstacles = new List<Entity>();
            _entityObstacles.Add(new Entity(Entity.Type.None));
            
            base.LoadContent();
        }

        private void ea_UpdateEvent(object sender, EventArgs e)
        {
            
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

            #region AI
            //with the offpring, evaluate
            var answers = birds.ToDictionary(
                x => ((NeatBrainController)x.Controller).Genome,
                x => new BirdEvaluator().Evaluate(x)
            );

            //set answers
            KnownAnswerListEvaluator<NeatGenome, IBlackBox> evaluator = new KnownAnswerListEvaluator<NeatGenome, IBlackBox>();
            evaluator.SetKnownAnswers(answers);

            //call the evaluator
            evaluator.Evaluate(offspringData.OffspringList);

            //Update the species
            _ea.UpdateSpecies(offspringData);

            //call callbacks :)
            _ea.PerformUpdateCallbacks();
            #endregion



            InitializeBirds(birds);
            //_entityBird = new Entities.Bird(Entities.Entity.Type.Bird);

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

        private void InitializeBirds(List<Bird> birds)
        {
            //start the generation
            _ea.StartGeneration();

            //create offpring
            offspringData = _ea.CreateOffpring();

            //create genome decoder
            var decoder = experiment.CreateGenomeDecoder();

            birds.Clear();

            //Statics.AmountOfBirds
            for (int i = 0; i < offspringData.OffspringList.Count; i++)
            {
                //birds.Add(new Entities.Bird(Entities.Entity.Type.Bird, new KeyboardController()));
                //birds.Add(new Entities.Bird(Entities.Entity.Type.Bird, new RandomController(), GetNewColor()));
                birds.Add(new Entities.Bird(Entities.Entity.Type.Bird, new NeatBrainController(
                        decoder.Decode(offspringData.OffspringList[i]),
                        offspringData.OffspringList[i]
                    ), GetNewColor()));
            }
        }

        private Color GetNewColor()
        {
            //return Color.Blue;
            return new Color(255 - RandSubstract(), 255 - RandSubstract(), 255 - RandSubstract());
        }

        private int RandSubstract()
        {
            return Statics.Random.Next(250);
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

                            foreach(var _entityBird in birds.Where(x => !x.IsDead))
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

                            

                            Statics.GAME_SCORE = birds.Max(x => x.Points);

                            if (obstacle.Position.X <= -obstacle.Width)
                                _entityObstacles.Remove(obstacle);

                            _isCheckingCollision = false;
                        }
                    }
                }

                if (birds.All(x => x.IsDead))
                    SetGameState(Statics.STATE.GameOver);

                if (Statics.GAME_SCORE > Statics.GAME_HIGHSCORE)
                {
                    Statics.GAME_HIGHSCORE = Statics.GAME_SCORE;
                    Statics.GAME_NEWHIGHSCORE = true;
                }

                Statics.DEBUG_ENTITIES = _entityObstacles.Count;
                

                foreach(var _entityBird in birds)
                {
                    Statics.DEBUG_PLAYER = _entityBird.Position;
                    _entityBird.Update();
                }

                foreach (var obstacle in _entityObstacles)
                {
                    obstacle.Update();
                }
            }

            base.Update();
        }

        public override void Draw()
        {
            foreach (var _entityBird in birds)
            {
                _entityBird.Draw();
            }

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
