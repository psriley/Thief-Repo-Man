﻿using Microsoft.Xna.Framework.Graphics;
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

        private Vector2 playerPosition;
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
            angle = (float)Math.PI;
            direction.X = (float)Math.Cos(angle);
            direction.Y = (float)Math.Sin(angle);
            Debug.WriteLine($"Initial angle: {angle}");
            Debug.WriteLine($"Initial direction: {direction}");
        }

        public void HandleInput(GameTime gameTime, KeyboardState keyboardState)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                angle -= 0.1f;
                //acceleration += direction * LINEAR_ACCELERATION;
                //angularAcceleration += ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                angle += 0.1f;
                //acceleration += direction * LINEAR_ACCELERATION;
                //angularAcceleration -= ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                velocity -= direction * speed;
                //acceleration += direction * LINEAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                velocity += direction * speed;
                //acceleration -= direction * (LINEAR_ACCELERATION * 2);
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
            direction.X = (float)Math.Cos(angle);
            direction.Y = (float)Math.Sin(angle);
            direction.Normalize();

            //velocity += acceleration * t;
            position += velocity * t;

            speed = MathHelper.Clamp(speed, 0.0f, MAX_SPEED);

            ApplyEngineForce();
            //ApplySteering();

            //Vector2.Clamp(acceleration, Vector2.Zero, Vector2.One);
        }

        void ApplyEngineForce()
        {
            // Create a force for the engine
            Vector2 engineForceVector = new Vector2((float)Math.Sin(angle - 90), 0);
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
            spriteBatch.Draw(texture, position, null, Color.White, angle, new Vector2((105/2), (67/2)), 1f, SpriteEffects.None, 0);
        }
    }
}
