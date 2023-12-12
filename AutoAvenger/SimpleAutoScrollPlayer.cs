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
        public float health;
        // damage done by a bullet
        public float bulletDamage;
        public Bullet Bullet;
        public List<Bullet> bullets = new List<Bullet>();

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

            bulletDamage = 1f;
        }

        public void LoadContent(ContentManager content)
        {
            carTexture = content.Load<Texture2D>("forward_car");

            Bullet = new Bullet(carTexture, carPosition);

            _bounds = new BoundingRectangle(carPosition, carTexture.Width, carTexture.Height);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var bullet in bullets)
            {
                bullet.Update(gameTime);

                // Remove bullets that are outside the screen or expired
                if (bullet.lifespanEnded)
                {
                    bullets.Remove(bullet);
                    break; // Exit the loop to avoid modifying the collection during iteration
                }
            }

            Debug.WriteLine($"Bullet count: {bullets.Count}");
        }

        public void HandleInput(
            GameTime gameTime, 
            KeyboardState currentKeyboardState, 
            KeyboardState priorKeyboardState, 
            MouseState currentMouseState,
            MouseState priorMouseState
        ){
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

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                priorMouseState.LeftButton == ButtonState.Released)
            {
                Debug.WriteLine("Shooting!");
                Shoot();
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

        //private void Shoot()
        //{
        //    var bullet = Bullet.Clone() as Bullet;
        //    // might want to offset this to make it look like the driver is shooting
        //    bullet.Position = this.carPosition;
        //    bullet.LinearVelocity = new Vector2(0,5f);
        //}

        private void Shoot()
        {
            var bullet = new Bullet(carTexture, carPosition); // Create a new Bullet instance
            bullet.timer = 0;
            bullet.linearVelocity = new Vector2(0, 5f);

            bullets.Add(bullet); // Add the bullet to the list
        }

        private void UpdateBounds()
        {
            _bounds.X = carPosition.X;
            _bounds.Y = carPosition.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw bullets
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

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
