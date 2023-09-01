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

namespace Thief_Repo_Man
{
    public class Player
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState currentKeyboardState;
        KeyboardState priorKeyboardState;
        MouseState currentMouseState;
        MouseState priorMouseState;
        GamePadState currentGamePadState;
        GamePadState priorGamePadState;

        /// <summary>
        /// The current direction
        /// </summary>
        public Vector2 Direction { get; private set; }

        /// <summary>
        /// If the user has request to end the game
        /// </summary>
        public bool Exit { get; private set; } = false;

        /// <summary>
        /// If the sprite needs to be flipped horizontally
        /// </summary>
        public bool hflipped = false;

        /// <summary>
        /// If the sprite needs to be flipped vertically
        /// </summary>
        public bool vflipped = false;

        private Vector2 playerPosition;
        private Texture2D texture;
        private Texture2D playerTexture;
        private Texture2D playerLeftTexture;
        private float speed = 200f;

        public Player(Vector2 position)
        {
            this.playerPosition = position;
        }

        public void Update(GameTime gameTime)
        {
            texture = playerTexture;

            #region Updating input state

            priorKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            priorGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(0);

            #endregion

            #region Direction input

            // Get position from the GamePad
            Direction = currentGamePadState.ThumbSticks.Right * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Get position from the Keyboard
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                currentKeyboardState.IsKeyDown(Keys.A))
            {
                Direction += new Vector2(-speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                hflipped = false;
                texture = playerLeftTexture;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                currentKeyboardState.IsKeyDown(Keys.D))
            {
                Direction += new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                hflipped = true;
                texture = playerLeftTexture;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                currentKeyboardState.IsKeyDown(Keys.W))
            {
                Direction += new Vector2(0, -speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                vflipped = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                currentKeyboardState.IsKeyDown(Keys.S))
            {
                Direction += new Vector2(0, speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                vflipped = true;
            }

            #endregion

            playerPosition += Direction;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player(drawn)");
            playerLeftTexture = content.Load<Texture2D>("player_left");
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;

            if (vflipped && hflipped)
            {
                spriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
            }
            else if (vflipped)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            else if (hflipped)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            //SpriteEffects spriteEffects = (vflipped) ? SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(
                texture,
                playerPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1.5f,
                spriteEffects,
                0f
            );
            //spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0, new Vector2(64, 64), 0.25f, spriteEffects, 0);
        }
    }
}
