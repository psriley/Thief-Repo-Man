//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using Microsoft.Xna.Framework.Graphics;

//namespace Thief_Repo_Man
//{
//    public class InputManager
//    {
//        KeyboardState currentKeyboardState;
//        KeyboardState priorKeyboardState;
//        MouseState currentMouseState;
//        MouseState priorMouseState;
//        GamePadState currentGamePadState;
//        GamePadState priorGamePadState;

//        /// <summary>
//        /// The current direction
//        /// </summary>
//        public Vector2 Direction { get; private set; }

//        /// <summary>
//        /// If the user has request to end the game
//        /// </summary>
//        public bool Exit { get; private set; } = false;

//        /// <summary>
//        /// If the sprite needs to be flipped horizontally
//        /// </summary>
//        public bool hflipped = false;

//        /// <summary>
//        /// If the sprite needs to be flipped vertically
//        /// </summary>
//        public bool vflipped = false;

//        private float speed = 200f;

//        public void Update(GameTime gameTime)
//        {
//            #region Updating input state

//            priorKeyboardState = currentKeyboardState;
//            currentKeyboardState = Keyboard.GetState();

//            priorMouseState = currentMouseState;
//            currentMouseState = Mouse.GetState();

//            priorGamePadState = currentGamePadState;
//            currentGamePadState = GamePad.GetState(0);

//            #endregion

//            #region Direction input

//            // Get position from the GamePad
//            Direction = currentGamePadState.ThumbSticks.Right * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

//            // Get position from the Keyboard
//            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
//                currentKeyboardState.IsKeyDown(Keys.A))
//            {
//                Direction += new Vector2(-speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
//            }
//            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
//                currentKeyboardState.IsKeyDown(Keys.D))
//            {
//                Direction += new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
//            }
//            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
//                currentKeyboardState.IsKeyDown(Keys.W))
//            {
//                Direction += new Vector2(0, -speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
//                vflipped = false;
//            }
//            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
//                currentKeyboardState.IsKeyDown(Keys.S))
//            {
//                Direction += new Vector2(0, speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
//                vflipped = true;
//            }

//            #endregion

//            #region Exit input

//            if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
//            {
//                Exit = true;
//            }

//            #endregion
//        }

//        /// <summary>
//        /// Draws the sprite using the supplied SpriteBatch
//        /// </summary>
//        /// <param name="gameTime">The game time</param>
//        /// <param name="spriteBatch">The spritebatch to render with</param>
//        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//            SpriteEffects spriteEffects = (vflipped) ? SpriteEffects.FlipVertically : SpriteEffects.None;
//            spriteBatch.Draw(texture, position, null, Color, 0, new Vector2(64, 64), 0.25f, spriteEffects, 0);
//        }
//    }
//}
