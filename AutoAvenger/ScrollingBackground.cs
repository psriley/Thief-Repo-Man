using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAvenger
{
    public class ScrollingBackground
    {
        public Texture2D backgroundTexture;
        public Rectangle backgroundRect;
        public int backgroundSpeed = 400;

        public ScrollingBackground(Texture2D newTexture, Rectangle newRectangle)
        { 
            backgroundTexture = newTexture;
            backgroundRect = newRectangle;
        }

        public void Update(GameTime gameTime)
        {
            // move background down by the speed every frame
            backgroundRect.Y += (int)(backgroundSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundRect, Color.White);
        }
    }
}
