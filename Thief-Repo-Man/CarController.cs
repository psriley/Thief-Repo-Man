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
using Thief_Repo_Man.Particle_System;

namespace Thief_Repo_Man
{
    public class CarController : IParticleEmitter
    {
        private float accelerationSpeed = 30.0f;
        private float turnSpeed = 3.5f;

        float acceleration = 0;
        float steering = 0;

        float rotationAngle = 0;

        private static Vector2 _direction;
        /// <summary>
        /// Direction that the player is facing.
        /// </summary>
        public static Vector2 Direction => _direction;

        public Vector2 startingCarPosition;
        private float speed = 200f;
        private BoundingRectangle bounds;
        private float playerScale = 1.5f;
        private float rotation = 0;

        public BoundingRectangle Bounds => bounds;

        const float LINEAR_ACCELERATION = 100;
        const float ANGULAR_ACCELERATION = 2;
        const float MAX_SPEED = 5;

        Game game;
        Texture2D texture;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        Vector2 direction;

        public float Rotation { get; set; }
        float angularVelocity;

        public CarController(Vector2 position)
        {
            this.Position = new Vector2(526, 356);
            //this.Position = position;
            this.bounds = new BoundingRectangle(startingCarPosition, (44 * playerScale), (22 * playerScale));
            _direction = -Vector2.UnitY;
            Rotation = (float)Math.PI;
            direction.X = (float)Math.Cos(Rotation);
            direction.Y = (float)Math.Sin(Rotation);
            Debug.WriteLine($"Initial rotation: {Rotation}");
            Debug.WriteLine($"Initial direction: {direction}");
        }

        public void HandleInput(GameTime gameTime, KeyboardState keyboardState)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Rotation -= 0.1f;
                //acceleration += direction * LINEAR_ACCELERATION;
                //angularAcceleration += ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Rotation += 0.1f;
                //acceleration += direction * LINEAR_ACCELERATION;
                //angularAcceleration -= ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Velocity -= direction * speed;
                //acceleration += direction * LINEAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                Velocity += direction * speed;
                //acceleration -= direction * (LINEAR_ACCELERATION * 2);
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Velocity = Vector2.Zero;
            }

            //if (velocity.Length() > 0f)
            //{
            //    acceleration -= direction * (LINEAR_ACCELERATION / 2);
            //}
            //if (acceleration.Length() > 0)
            //{
            //    acceleration -= direction * LINEAR_ACCELERATION;
            //    velocity -= acceleration * t;
            //}

            //angularVelocity += angularAcceleration * t;
            //angle += angularVelocity * t;
            direction.X = (float)Math.Cos(Rotation);
            direction.Y = (float)Math.Sin(Rotation);
            direction.Normalize();

            //velocity += acceleration * t;
            Position += Velocity * t;

            speed = MathHelper.Clamp(speed, 0.0f, MAX_SPEED);

            ApplyEngineForce();
            //ApplySteering();

            //Vector2.Clamp(acceleration, Vector2.Zero, Vector2.One);
        }

        void ApplyEngineForce()
        {
            // Create a force for the engine
            Vector2 engineForceVector = new Vector2((float)Math.Sin(Rotation - 90), 0);
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
            
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2((105/2), (67/2)), 1f, SpriteEffects.None, 0);
        }
    }
}
