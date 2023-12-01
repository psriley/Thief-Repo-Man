using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AutoAvenger
{
    public class SimpleAutoScrollPlayer
    {
        public Texture2D carTexture;
        public Vector2 carPosition;

        private Vector2 _origin;
        private float _initialRotation;
        private float _speed;

        private Vector2 _direction;
        public Vector2 Direction => _direction;

        public SimpleAutoScrollPlayer(Vector2 carPosition)
        {
            this.carPosition = carPosition;

            _direction = Vector2.Zero;
            _initialRotation = MathHelper.PiOver2;
            _speed = 250f;
        }

        public void LoadContent(ContentManager content)
        {
            carTexture = content.Load<Texture2D>("car");
            _origin = new Vector2(carTexture.Width / 2, carTexture.Height / 2);
        }

        public void HandleInput(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            _direction = Vector2.Zero;

            if (currentKeyboardState.IsKeyDown(Keys.W)) { _direction.Y--; }
            else if (currentKeyboardState.IsKeyDown(Keys.S)) { _direction.Y++; }
            if (currentKeyboardState.IsKeyDown(Keys.A)) { _direction.X--; }
            else if (currentKeyboardState.IsKeyDown(Keys.D)) { _direction.X++; }

            // Check if the direction vector is not a zero vector before normalizing
            if (_direction != Vector2.Zero)
            {
                _direction.Normalize();
                carPosition += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(carTexture, carPosition, Color.White);
            spriteBatch.Draw(
                carTexture,
                carPosition,
                null,
                Color.White,
                _initialRotation,
                _origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}
