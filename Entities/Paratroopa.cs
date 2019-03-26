using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace FlappyBird.Entities
{
    class Paratroopa : Entity
    {
        private AnimatedSprite _paratroopa_Sprite;
        private Vector2 _originPos;
        private Vector2 _destinationPos;
        private double _dirAngle;
        private double _oscAngle;
        private float _amp = 2;
        private float _baseSpeed = 3.5f;

        private int _soundCounter;
        private int _soundFrequency;

        const double FREQUENCY = Math.PI / 75;

        public Paratroopa(Type type, float speedModifier)
            : base(type)
        {
            this.Texture = ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_paratroopa");
            this.Position = new Vector2(Statics.GAME_WIDTH + this.Texture.Width, Statics.GAME_RANDOM.Next(Statics.GAME_HEIGHT / 2, Statics.GAME_FLOOR));
            this.Width = this.Texture.Width;
            this.Height = this.Texture.Height;
            this.EntityType = type;
            this.MoveSpeed = _baseSpeed + speedModifier + Statics.GAME_RANDOM.Next(0, 3);
            this.ColorData = new Color[this.Width * this.Height];
            this.Texture.GetData(ColorData);

            Texture2D animated_Texture = ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_paratroopa_animated");

            _paratroopa_Sprite = new AnimatedSprite();
            _paratroopa_Sprite.Initialize(animated_Texture, this.Position, this.Rotation, 128, 128, 4, 60, Color.White, this.Scale, true);

            _originPos = this.Position;
            _destinationPos = new Vector2(-this.Width, this.Position.Y);
            _dirAngle = Math.Atan2(_destinationPos.Y - _originPos.Y, _destinationPos.X - _originPos.X);
            _amp = Statics.GAME_RANDOM.Next(4, 8);

            _soundCounter = 0;
            _soundFrequency = 60;
        }

        public override void Update()
        {
            if (Statics.GAME_STATE == Statics.STATE.Playing)
            {
                if (Statics.GAME_USESLOWMODE)
                    this.Position.X -= this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY * Statics.GAME_SLOWMODERATE;
                else
                    this.Position.X -= this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY;

                _oscAngle += FREQUENCY;

                var oscDelta = Math.Sin(_oscAngle);
                var stepVector = new Vector2((float)Math.Cos(_dirAngle) * this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY, (float)(Math.Sin(_dirAngle) * this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY));
                var oscNormalAngle = _dirAngle + Math.PI / 2;
                var oscVector = new Vector2((float)(Math.Cos(oscNormalAngle) + oscDelta * _amp), (float)(Math.Sin(oscNormalAngle) * oscDelta) * _amp);

                this.Position.Y += stepVector.Y + oscVector.Y;

                this.Position.Y = MathHelper.Clamp(this.Position.Y, 0, Statics.GAME_FLOOR - this.Height * this.Scale);

                _paratroopa_Sprite.Position = this.Position;
                _paratroopa_Sprite.Update(Statics.GAME_GAMETIME);

                if (_soundCounter > _soundFrequency)
                {
                    Statics.MANAGER_SOUND.Play("Paratroopa\\Jump");
                    _soundCounter = 0;
                }

                _soundCounter++;
            }
        }

        public override void Draw()
        {
            _paratroopa_Sprite.Draw(Statics.GAME_SPRITEBATCH);

            base.Draw();
        }
    }
}
