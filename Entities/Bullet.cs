using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird.Entities
{
    class Bullet : Entity
    {
        private float _baseSpeed = 12;

        public Bullet(Type type, float speedModifier)
            : base(type)
        {
            this.Texture = ParallaxBackground.GetFromPng("Content\\Textures\\Entity\\flappy_bullet");
            this.Position = new Vector2(Statics.GAME_WIDTH, Statics.GAME_RANDOM.Next((int)Statics.DEBUG_PLAYER.Y - (int)((this.Texture.Height / 2) * this.Scale), (int)Statics.DEBUG_PLAYER.Y - (int)((this.Texture.Height / 4) * this.Scale)));
            this.Width = this.Texture.Width;
            this.Height = this.Texture.Height;
            this.EntityType = type;
            this.MoveSpeed = _baseSpeed + speedModifier;
            this.ColorData = new Color[this.Width * this.Height];
            this.Texture.GetData(ColorData);
        }

        public override void Update()
        {
            if (Statics.GAME_STATE == Statics.STATE.Playing)
            {
                if (Statics.GAME_USESLOWMODE)
                    this.Position.X -= this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY * Statics.GAME_SLOWMODERATE;
                else
                    this.Position.X -= this.MoveSpeed * Statics.GAME_SPEED_DIFFICULTY;

                this.Position.Y = MathHelper.Clamp(this.Position.Y, 0, Statics.GAME_FLOOR - this.Height * this.Scale);
            }
        }

        public override void Draw()
        {
            Statics.GAME_SPRITEBATCH.Begin();
            Statics.GAME_SPRITEBATCH.Draw(this.Texture, this.Position, Color.White);
            Statics.GAME_SPRITEBATCH.End();

            base.Draw();
        }
    }
}
