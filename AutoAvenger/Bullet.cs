using AutoAvenger.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAvenger
{
    public class Bullet
    {
        public Vector2 position;
        public Vector2 linearVelocity;
        public bool lifespanEnded;
        public float timer;

        private BoundingRectangle _bounds;
        public BoundingRectangle Bounds => _bounds;

        private Texture2D texture;
        private float lifespan = 2f;

        public Bullet(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            _bounds = new BoundingRectangle(this.position, this.texture.Width, this.texture.Height);
        }

        public void Update(GameTime gameTime, bool up)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= lifespan)
            {
                lifespanEnded = true;
            }


            if (up)
            {
                position -= Vector2.UnitY * linearVelocity;
            }
            else
            {
                position += Vector2.UnitY * linearVelocity;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
