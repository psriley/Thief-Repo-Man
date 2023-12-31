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
using AutoAvenger.Collisions;
using AutoAvenger.Particle_System;

namespace AutoAvenger
{
    public class CarController : IParticleEmitter
    {
        const float LINEAR_ACCELERATION = 100;
        const float ANGULAR_ACCELERATION = 2;
        const float MAX_SPEED = 5;
        const float ROTATIONAL_VELOCITY = 3f;
        const float LINEAR_VELOCITY = 4f;

        public Vector2 origin;

        //private float accelerationSpeed = 30.0f;
        //private float turnSpeed = 3.5f;

        //float acceleration = 0;
        //float steering = 0;

        //float rotationAngle = 0;
        //float angularVelocity;

        private static Vector2 _direction;
        /// <summary>
        /// Direction that the player's car is facing.
        /// </summary>
        public static Vector2 Direction => _direction;

        private Vector2 _position;
        /// <summary>
        /// Position of the player's car.
        /// </summary>
        public Vector2 Position => _position;

        private BoundingRectangle _bounds;
        /// <summary>
        /// The bounds for the player's car.
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The car that this controller is controlling.
        /// </summary>
        public Car car;

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-origin, 0)) *
                    Matrix.CreateRotationZ(_rotation) *
                    Matrix.CreateTranslation(new Vector3(_position, 0));
            }
        }

        private float _speed = 200f;
        private float _playerScale = 1.5f;
        private float _rotation = 0;
        private Vector2 _velocity;

        Game game;
        Texture2D texture;

        // particle emitter variables
        public Vector2 EmitterPosition { get; set; }
        public Vector2 EmitterVelocity { get; set; }
        public float EmitterRotation { get; set; }

        //private Vector2 _emitterOffset = new Vector2(526, 356);

        public CarController()
        {
            //this.car = car;
            //_position = car.CarPosition;
            //_bounds = car.Bounds;
            _rotation = (float)Math.PI;
            _direction = -Vector2.UnitY;
            _rotation = (float)Math.PI;
            _direction.X = (float)Math.Cos(_rotation);
            _direction.Y = (float)Math.Sin(_rotation);
            _velocity = Vector2.Zero;
            //_position = position;
            //_bounds = new BoundingRectangle(_position, (44 * _playerScale), (22 * _playerScale));
            Debug.WriteLine($"Initial rotation: {_rotation}");
            Debug.WriteLine($"Initial direction: {_direction}");

            //// set emitter properties
            //EmitterPosition = _position;
            //EmitterRotation = _rotation;
            //EmitterVelocity = _velocity;
        }

        public void SetCarToControl(Car car)
        {
            this.car = car;
            _position = car.CarPosition;
            _bounds = car.Bounds;

            // set emitter properties
            EmitterPosition = _position;
            EmitterRotation = _rotation;
            EmitterVelocity = _velocity;
        }

        public void HandleInput(GameTime gameTime, KeyboardState keyboardState)
        {
            if (car.isDestroyed)
            {
                return;
            }

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Vector2 acceleration = new Vector2(0, 0);
            //float angularAcceleration = 0;

            if (keyboardState.IsKeyDown(Keys.A))
            {
                _rotation -= MathHelper.ToRadians(ROTATIONAL_VELOCITY);

                //_rotation -= 0.1f;

                //acceleration += direction * LINEAR_ACCELERATION;
                //angularAcceleration += ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                _rotation += MathHelper.ToRadians(ROTATIONAL_VELOCITY);

                //_rotation += 0.1f;

                //acceleration += direction * LINEAR_ACCELERATION;
                //angularAcceleration -= ANGULAR_ACCELERATION;
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

            _direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
            //// prevent diagonal movement from being faster than other movement.
            //_direction.Normalize();


            if (keyboardState.IsKeyDown(Keys.W))
            {
                _position += _direction * LINEAR_VELOCITY;

                //_velocity -= _direction * _speed;

                //acceleration += direction * LINEAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                _position -= _direction * LINEAR_VELOCITY;

                //_velocity += _direction * _speed;

                //acceleration -= direction * (LINEAR_ACCELERATION * 2);
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _velocity = Vector2.Zero;
            }

            //velocity += acceleration * t;
            ////Position += Velocity * t;
            //_position += _velocity * t;

            //_position = Vector2.Clamp(_position, new Vector2(0, 0), new Vector2(game))

            // Update the bounds to sync with position of the car sprite
            _bounds.X = _position.X - car.CarTexture.Width / 2;
            _bounds.Y = _position.Y - car.CarTexture.Height / 2;
            _bounds.Width = car.CarTexture.Width;
            _bounds.Height = car.CarTexture.Height;

            // clamp the speed of the car so it never exceeds the set max speed.
            _speed = MathHelper.Clamp(_speed, 0.0f, MAX_SPEED);

            //ApplyEngineForce();
            //ApplySteering();

            //Vector2.Clamp(acceleration, Vector2.Zero, Vector2.One);

            // sync particle system with player's car.
            //EmitterPosition = _position;
            //EmitterPosition = _position;
            EmitterRotation = _rotation;
            EmitterVelocity = -_velocity;
        }

        //void ApplyEngineForce()
        //{
        //    // Create a force for the engine
        //    Vector2 engineForceVector = new Vector2((float)Math.Sin(_rotation - 90), 0);
        //}

        /// <summary>
        /// Loads the sprite texture
        /// </summary>
        /// <param name="content">The content manager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("car");
        }

        /// <summary>
        /// Updates the car sprite
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
            spriteBatch.Draw(texture, _position, null, Color.White, _rotation, origin, 1f, SpriteEffects.None, 0);
            // Debug size of collider
            var rect = new Rectangle((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
            spriteBatch.Draw(car.CarTexture, rect, Color.Red);
        }
    }
}
