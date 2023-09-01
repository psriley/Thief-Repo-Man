using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Thief_Repo_Man.Collisions;

namespace Thief_Repo_Man
{
    public class Car
    {
        private Vector2 carPosition;
        private Texture2D carTexture;
        private BoundingRectangle bounds;

        public BoundingRectangle Bounds => bounds;
        
        public Car(Vector2 position)
        {
            this.carPosition = position;
            this.bounds = new BoundingRectangle(carPosition, 114, 114);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            carTexture = content.Load<Texture2D>("car");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                carTexture,
                carPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
            //// Debug size of collider
            //var rect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            //spriteBatch.Draw(carTexture, rect, Color.Red);
        }
    }
}
