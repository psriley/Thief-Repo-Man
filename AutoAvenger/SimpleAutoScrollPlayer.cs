using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAvenger.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;

namespace AutoAvenger
{
    public class SimpleAutoScrollPlayer
    {
        public Texture2D carTexture;
        public Vector2 carPosition;
        public int health;

        private Vector2 _origin;
        //private float _initialRotation;
        private float _speed;
        private BoundingRectangle _bounds;
        public BoundingRectangle Bounds => _bounds;

        private Vector2 _direction;
        public Vector2 Direction => _direction;
        public bool Jumping;

        float jumpTimer;
        float jumpDuration = 3.0f;
        float maxCarSize = 1.5f; // max size as the car jumps

        float carScale = 1.0f; // initial scale of the car

        public SimpleAutoScrollPlayer(Vector2 carPosition)
        {
            this.carPosition = carPosition;

            _direction = Vector2.Zero;
            //_initialRotation = MathHelper.PiOver2;
            _speed = 250f;
            Jumping = false;
            jumpTimer = 0f;
        }

        public void LoadContent(ContentManager content)
        {
            carTexture = content.Load<Texture2D>("forward_car");

            _bounds = new BoundingRectangle(carPosition, carTexture.Width, carTexture.Height);
        }

        public void HandleInput(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState priorKeyboardState)
        {
            _direction = Vector2.Zero;

            if (currentKeyboardState.IsKeyDown(Keys.W)) { _direction.Y--; }
            else if (currentKeyboardState.IsKeyDown(Keys.S)) { _direction.Y++; }
            if (currentKeyboardState.IsKeyDown(Keys.A)) { _direction.X--; }
            else if (currentKeyboardState.IsKeyDown(Keys.D)) { _direction.X++; }

            if (currentKeyboardState.IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space) && !Jumping)
            {
                Jump();
            }
            else if (Jumping)
            {
                jumpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jumpTimer < (jumpDuration / 2))
                {
                    // increase the size of the car as it ascends
                    float progress = jumpTimer / (jumpDuration / 2);
                    carScale = MathHelper.Lerp(1.0f, maxCarSize, progress);
                }
                else
                {
                    // Decrease the size of the car back to its initial size as it descends
                    float progress = (jumpTimer - 1.5f) / (jumpDuration - 1.5f);
                    carScale = MathHelper.Lerp(maxCarSize, 1.0f, progress);
                }

                if (jumpTimer >= jumpDuration)
                {
                    Jumping = false;
                }
            }

            // Check if the direction vector is not a zero vector before normalizing
            if (_direction != Vector2.Zero)
            {
                _direction.Normalize();
                carPosition += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                UpdateBounds();
            }
        }

        private void Jump()
        {
            Debug.WriteLine("Jumping!");
            // Turn off handle input (can't move in the air)?
            // Turn off collision (can jump over obstacles)
            // Make car bigger and darker as you jump, and then smaller and lighter as you land
            Jumping = true;
            jumpTimer = 0f;
            // Reset car scale at the beginning of the jump
            carScale = 1f;
        }

        private void UpdateBounds()
        {
            _bounds.X = carPosition.X;
            _bounds.Y = carPosition.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(carTexture, carPosition, Color.White);
            spriteBatch.Draw(
                carTexture,
                carPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                carScale,
                SpriteEffects.None,
                0f
            );

            //DebugCollider(spriteBatch);
        }

        private void DebugCollider(SpriteBatch spriteBatch)
        {
            // Debug size of collider
            var rect = new Rectangle((int)_bounds.X, (int)_bounds.Y, (int)_bounds.Width, (int)_bounds.Height);
            spriteBatch.Draw(
                carTexture, // You can replace this with an appropriate debug texture if needed
                rect,
                null,
                Color.Red,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );
        }
    }
}
