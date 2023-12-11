using AutoAvenger.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAvenger
{
    public class Obstacle
    {
        public Vector2 position;
        public Texture2D obstacleTexture;
        public Texture2D debugTexture;
        public Texture2D damagedTexture;
        public BoundingRectangle bounds;
        public bool isDestroyed;

        public Obstacle(ScrollingBackground background, Texture2D obstacleTexture, Texture2D debugTexture, Texture2D damagedTexture, Vector2? position = null)
        {
            if (position == null)
            {
                // generate a random position
                Random rand = new();
                this.position = new Vector2(rand.Next(0, background.backgroundRect.Width), rand.Next(0, background.backgroundRect.Height));
            }
            else
            {
                this.position = (Vector2)position;
            }

            // bounds start at the origin of the position of this obstacle
            this.bounds = new BoundingRectangle(new Vector2((this.position.X / 2), (this.position.Y / 2)), obstacleTexture.Width, obstacleTexture.Height);

            this.obstacleTexture = obstacleTexture;
            this.damagedTexture = damagedTexture;
            this.debugTexture = debugTexture;
        }

        public void DamageCar(SimpleAutoScrollPlayer car)
        {
            // destroy this obstacle so it only damages the player once
            this.isDestroyed = true;

            car.health -= 10;

            // Obstacle and player get damaged (this way obstacles can't damage multiple times and it's clear that they hit it)
            Debug.WriteLine($"Collision! {this.position}");
        }

        public void Update(GameTime gameTime, int backgroundSpeed)
        {
            // move background down by the speed every frame
            position.Y += (int)(backgroundSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // don't need to update bounds if this obstacle is destroyed (less computation).
            if (!isDestroyed)
            {
                bounds.X = position.X; // Update X
                bounds.Y = position.Y; // Update Y
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDestroyed)
            {
                spriteBatch.Draw(damagedTexture, position, Color.White);
            }
            else
            {
                spriteBatch.Draw(obstacleTexture, position, Color.White);
            }

            // Debug size of collider
            //var boundingRect = new BoundingRectangle(new Vector2((bounds.X / 2), (bounds.Y / 2)), obstacleTexture.Width, obstacleTexture.Height);
            
            //var rect = (Rectangle)boundingRect;
            //var rect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            //spriteBatch.Draw(obstacleTexture, rect, Color.Red);
        }
    }
}
