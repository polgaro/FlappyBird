using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird.Entities
{
    class Pipe : Entity
    {
        private float _baseSpeed = 4f;

        public Pipe(Type type, float speedModifier)
            : base(type)
        {
            this.DeadTexture = ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_pipe");
            this.Position = new Vector2(Statics.GAME_WIDTH, Statics.GAME_RANDOM.Next(Statics.GAME_HEIGHT / 4 - this.DeadTexture.Height / 2, Statics.GAME_HEIGHT / 4 - this.DeadTexture.Height / 4));
            this.Width = this.DeadTexture.Width;
            this.Height = this.DeadTexture.Height;
            this.EntityType = type;
            this.MoveSpeed = _baseSpeed + speedModifier;
            this.ColorData = new Color[this.Width * this.Height];
            this.DeadTexture.GetData(ColorData);
        }

        public override void Update()
        {
            if (Statics.GAME_STATE == Statics.STATE.Playing)
            {
                if (Statics.GAME_USESLOWMODE)
                    this.Position.X -= this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY * Statics.GAME_SLOWMODERATE;
                else
                    this.Position.X -= this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY;
            }
        }

        public override void Draw()
        {
            Statics.GAME_SPRITEBATCH.Begin();
            Statics.GAME_SPRITEBATCH.Draw(this.DeadTexture, this.Position, Color.White);
            Statics.GAME_SPRITEBATCH.End();

            base.Draw();
        }
    }
}
