using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAvenger
{
    public class Obstacle
    {
        public Vector2 position;
        public Texture2D obstacleTexture;

        public Obstacle(ScrollingBackground background, Texture2D obstacleTexture, Vector2? position = null)
        {
            if (position == null)
            {
                // generate a random position
                Random rand = new();
                position = new Vector2(rand.Next(0, background.backgroundRect.Width), rand.Next(0, background.backgroundRect.Height));
            }
            else
            {
                this.position = (Vector2)position;
            }

            this.obstacleTexture = obstacleTexture;
        }

        public void Update(GameTime gameTime, int backgroundSpeed)
        {
            // move background down by the speed every frame
            position.Y += (int)(backgroundSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(obstacleTexture, position, Color.Blue);
        }
    }
}
