using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Diagnostics;
using SharpDX.Direct3D9;
using Microsoft.Xna.Framework.Content;
using Thief_Repo_Man.Collisions;

namespace Thief_Repo_Man
{
    public class CarController
    {
        private static Vector2 _direction;
        /// <summary>
        /// Direction that the player is facing.
        /// </summary>
        public static Vector2 Direction => _direction;

        private Vector2 playerPosition;
        private float speed = 200f;
        private BoundingRectangle bounds;
        private float playerScale = 1.5f;

        public BoundingRectangle Bounds => bounds;

        const float LINEAR_ACCELERATION = 10;
        const float ANGULAR_ACCELERATION = 2;

        Game game;
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        Vector2 direction;

        float angle;
        float angularVelocity;

        public CarController(Vector2 position)
        {
            this.position = new Vector2(526, 356);
            this.bounds = new BoundingRectangle(playerPosition, (44 * playerScale), (22 * playerScale));
            _direction = -Vector2.UnitY;
        }

        public void HandleInput(GameTime gt, KeyboardState ks)
        {
            Update(gt, ks);
        }

        /// <summary>
        /// Loads the sprite texture
        /// </summary>
        /// <param name="content">The content manager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("car");
        }

        /// <summary>
        /// Updates the ship sprite
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration += ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration -= ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                acceleration += direction * LINEAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                acceleration -= direction * LINEAR_ACCELERATION;
            }
            //if (acceleration.Length() > 0)
            //{
            //    acceleration -= direction * LINEAR_ACCELERATION;
            //    velocity -= acceleration * t;
            //}

            angularVelocity += angularAcceleration * t;
            angle += angularVelocity * t;
            direction.X = (float)Math.Sin(angle - 90);
            direction.Y = (float)-Math.Cos(angle -90);

            velocity += acceleration * t;
            position += velocity * t;

            //Vector2.Clamp(acceleration, Vector2.Zero, Vector2.One);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, angle, new Vector2((105/2), (67/2)), 1f, SpriteEffects.None, 0);
        }
    }
}
