using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlappyBird.AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird.Entities
{
    class Bird : Entity
    {
        private AnimatedSprite _bird_Sprite;
        private float _ySpeed;

        public bool UseSlowFall = false;
        public bool UseJumpBoost = false;
        public bool UseStarPower = false;

        public bool IsDead = false;
        public int Points = 0;

        public IController Controller { get; }

        public Bird(Type type, IController controller) : base(type)
        {
            this.Texture = Statics.MANAGER_TEXTURES.Textures["Entity\\DeadBird"];
            this.Position = new Vector2(300, 300);
            this.Rotation = 0f;
            this.Scale = .75f;
            this.EntityType = type;
            this.Width = this.Texture.Width;
            this.Height = this.Texture.Height;
            this.MoveSpeed = 5f;
            this.ColorData = new Color[this.Width * this.Height];
            this.Texture.GetData(ColorData);
            this.Controller = controller;

            Texture2D texture = Statics.MANAGER_TEXTURES.AnimatedTextures["Entity\\Bird"];

            _bird_Sprite = new AnimatedSprite();
            _bird_Sprite.Initialize(texture, this.Position, this.Rotation, 128, 128, 4, 60, Color.White, this.Scale, true);

            _ySpeed = 0f;
        }

        public override void Update()
        {
            if (Statics.GAME_STATE == Statics.STATE.Playing && !this.IsDead)
            {
                _ySpeed += UseSlowFall ? Statics.ySpeedSlowFall : Statics.ySpeedNormalFall;

                CheckForInput();

                this.Position.Y =  MathHelper.Clamp(this.Position.Y, (this.Height * this.Scale), Statics.GAME_FLOOR + this.Height * _bird_Sprite.Scale);

                if (this.Position.Y < Statics.GAME_FLOOR)
                {
                    this.Position.Y += _ySpeed;
                }
                else
                {
                    this.IsDead = true;
                }

                this.Rotation = (float)Math.Atan2(_ySpeed, 10);

                _bird_Sprite.Position = this.Position;
                _bird_Sprite.Rotation = this.Rotation;
                _bird_Sprite.Update(Statics.GAME_GAMETIME);
            }
        }

        public override void Draw()
        {
            if (Statics.GAME_STATE == Statics.STATE.GameOver)
            {
                Statics.GAME_SPRITEBATCH.Begin();
                Statics.GAME_SPRITEBATCH.Draw(this.Texture, _bird_Sprite.Bounds, new Rectangle(0, 0, _bird_Sprite.FrameWidth, _bird_Sprite.FrameHeight), Color.White, _bird_Sprite.Rotation, _bird_Sprite.SourceRotate, SpriteEffects.None, 1.0f);
                Statics.GAME_SPRITEBATCH.End();
            }
            else
            {
                _bird_Sprite.Draw(Statics.GAME_SPRITEBATCH);
            }

            base.Draw();
        }

        private void CheckForInput()
        {
            if (Controller.WantsToJump())
                Jump();

            if (Controller.WantsJumpBoost()) 
                UseJumpBoost = UseJumpBoost ? false : true;

            if (Controller.WantsSlowFall())
                UseSlowFall = UseSlowFall ? false : true;

            // Input : Gamepad
            this.Position.X += Statics.MANAGER_INPUT.CurrentGamePadState().ThumbSticks.Left.X * this.MoveSpeed;
        }

        private void Jump()
        {
            Statics.MANAGER_SOUND.Play("Player\\Jump");

            _ySpeed = UseJumpBoost ? Statics.BoostedJumpHeight : Statics.NormalJumpHeight;
        }
    }
}
