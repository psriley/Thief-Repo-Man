using AutoAvenger.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoAvenger
{
    public class EnemyCar
    {
        private BoundingRectangle _bounds;
        public BoundingRectangle Bounds => _bounds;

        private Texture2D _enemyTexture;
        public float _enemyHealth;
        public float maxEnemyHealth;
        private Vector2 _playerPosition;
        public Vector2 _followerPosition;
        private SimpleAutoScrollPlayer _playerCar;
        public float followSpeed = 200f;
        public List<Bullet> bullets = new List<Bullet>();
        public float shotTimer;

        public bool isDestroyed;

        public EnemyCar(Texture2D texture, SimpleAutoScrollPlayer playerToFollow)
        {
            _enemyTexture = texture;
            _playerCar = playerToFollow;
            UpdatePlayerPosition(); // Initialize the player position
            _followerPosition = new Vector2(_playerCar.carPosition.X, 720 / 10f);

            shotTimer = 0;
            _enemyHealth = maxEnemyHealth;
            isDestroyed = false;
            _bounds = new BoundingRectangle(_followerPosition, _enemyTexture.Width, _enemyTexture.Height);
        }

        private void UpdatePlayerPosition()
        {
            _playerPosition = _playerCar.carPosition;
        }

        public void Update(GameTime gameTime)
        {
            shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (shotTimer >= 3)
            {
                shotTimer = 0;
                var bullet = new Bullet(_enemyTexture, _followerPosition); // Create a new Bullet instance
                bullet.timer = 0;
                bullet.linearVelocity = new Vector2(0, 12f);

                bullets.Add(bullet); // Add the bullet to the list
            }

            foreach (var bullet in bullets)
            {
                bullet.Update(gameTime, false);

                // Remove bullets that are outside the screen or expired
                if (bullet.lifespanEnded)
                {
                    bullets.Remove(bullet);
                    break; // Exit the loop to avoid modifying the collection during iteration
                }
            }

            _bounds.X = _followerPosition.X;
            _bounds.Y = _followerPosition.Y;

            //UpdatePlayerPosition(); // Update the player position

            // Update follower position with a delay
            //float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //_followerPosition.X = MathHelper.Lerp(_followerPosition.X, _playerPosition.X, deltaTime * followSpeed);

            // Rest of your update logic...
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDestroyed) return;
            spriteBatch.Begin();
            // Draw bullets
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            spriteBatch.Draw(_enemyTexture, _followerPosition, Color.Red);
            spriteBatch.End();
        }
    }
}
