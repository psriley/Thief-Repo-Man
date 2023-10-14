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
using Thief_Repo_Man.Collisions;

namespace Thief_Repo_Man
{
    public enum Direction
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3,
    }

    public enum WalkingTexture
    {
        ForwardL = 0,
        ForwardR = 1,
        BackwardL = 2,
        BackwardR = 3,
        LeftL = 4,
        LeftR = 5,
        RightL = 6,
        RightR = 7,
    }

    public class CharacterController
    {
        private double animationTimer;
        private int animationIndex;
        private Tuple<int, int> forwardIndexRange = new Tuple<int, int>((int)WalkingTexture.ForwardL, (int)WalkingTexture.ForwardR);
        private Tuple<int, int> leftIndexRange = new Tuple<int, int>((int)WalkingTexture.LeftL, (int)WalkingTexture.LeftR);
        private Tuple<int, int> rightIndexRange = new Tuple<int, int>((int)WalkingTexture.RightL, (int)WalkingTexture.RightR);
        private Tuple<int, int> backwardIndexRange = new Tuple<int, int>((int)WalkingTexture.BackwardL, (int)WalkingTexture.BackwardR);
        private Tuple<int, int> currentIndexRange;

        private static Vector2 _direction;
        /// <summary>
        /// Direction that the player is facing.
        /// </summary>
        public static Vector2 Direction => _direction;

        /// <summary>
        /// If the sprite needs to be flipped horizontally
        /// </summary>
        public bool hflipped = false;

        /// <summary>
        /// If the sprite needs to be flipped vertically
        /// </summary>
        public bool vflipped = false;

        List<Keys> movementKeys = new List<Keys>
        {
            Keys.W,
            Keys.A,
            Keys.S,
            Keys.D,
            Keys.Up,
            Keys.Left,
            Keys.Down,
            Keys.Right,
        };

        private Texture2D playerTexture;
        private Texture2D idleTexture;
        private Texture2D forwardLTexture;
        private Texture2D forwardRTexture;
        private Texture2D playerLeftTexture;
        private Texture2D leftLTexture;
        private Texture2D leftRTexture;
        private bool moving;

        public Vector2 playerPosition;
        private float speed = 200f;
        private BoundingRectangle bounds;
        private float playerScale = 1.5f;

        public BoundingRectangle Bounds => bounds;

        public CharacterController(Vector2 position)
        {
            this.playerPosition = position;
            this.bounds = new BoundingRectangle(playerPosition, (44 * playerScale), (22 * playerScale));
            animationIndex = 0;
            currentIndexRange = forwardIndexRange;
            _direction = -Vector2.UnitY;
        }

        public void HandleInput(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            #region Direction input

            // Get position from the GamePad
            //_direction = currentGamePadState.ThumbSticks.Right * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            bool isMovementKeyPressed = movementKeys.Any(key => currentKeyboardState.IsKeyDown(key));

            if (isMovementKeyPressed)
            {
                // check specific key pressed and assign animation index and direction
                moving = true;
                _direction = Vector2.Zero;

                if (currentKeyboardState.IsKeyDown(Keys.W)) { _direction.Y--; currentIndexRange = forwardIndexRange; }
                else if (currentKeyboardState.IsKeyDown(Keys.S)) { _direction.Y++; currentIndexRange = backwardIndexRange; }
                if (currentKeyboardState.IsKeyDown(Keys.A)) { _direction.X--; currentIndexRange = leftIndexRange; }
                else if (currentKeyboardState.IsKeyDown(Keys.D)) { _direction.X++; currentIndexRange = rightIndexRange; }
            }
            else
            {
                // set moving to false
                moving = false;


                if (_direction == Vector2.UnitX || _direction == -Vector2.UnitX)
                {
                    playerTexture = playerLeftTexture;
                }
                else if (_direction == Vector2.UnitY || _direction == -Vector2.UnitY)
                {
                    playerTexture = idleTexture;
                }
            }

            //// Get position from the Keyboard
            //if (currentKeyboardState.IsKeyDown(Keys.Left) ||
            //    currentKeyboardState.IsKeyDown(Keys.A))
            //{
            //    Direction += new Vector2(-speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            //    hflipped = false;
            //    texture = playerLeftTexture;
            //}
            //if (currentKeyboardState.IsKeyDown(Keys.Right) ||
            //    currentKeyboardState.IsKeyDown(Keys.D))
            //{
            //    Direction += new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            //    hflipped = true;
            //    texture = playerLeftTexture;
            //}
            //if (currentKeyboardState.IsKeyDown(Keys.Up) ||
            //    currentKeyboardState.IsKeyDown(Keys.W))
            //{
            //    if (priorKeyboardState.IsKeyUp(Keys.Up) || priorKeyboardState.IsKeyUp(Keys.W))
            //    {
            //        movingForward = true;
            //    }

            //    Direction += new Vector2(0, -speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //    //texture = ;
            //    //vflipped = false;
            //}
            //if (currentKeyboardState.IsKeyDown(Keys.Down) ||
            //    currentKeyboardState.IsKeyDown(Keys.S))
            //{
            //    Direction += new Vector2(0, speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //    //vflipped = true;
            //}

            #endregion

            if (moving)
            {
                // Update animation timer
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                bool isInRange = animationIndex >= currentIndexRange.Item1 && animationIndex <= currentIndexRange.Item2;
                if (!isInRange)
                {
                    animationIndex = currentIndexRange.Item1;
                }

                // Update animation frame
                if (animationTimer > 0.25f)
                {
                    animationIndex++;
                    if (animationIndex > currentIndexRange.Item2) animationIndex = currentIndexRange.Item1;
                    animationTimer -= 0.25f;
                }
            }

            if (moving)
            {
                playerPosition += Vector2.Normalize(Direction) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Update the bounds to sync with position of the sprite
            bounds.X = playerPosition.X;
            bounds.Y = playerPosition.Y;
            bounds.Width = (currentIndexRange == forwardIndexRange || currentIndexRange == backwardIndexRange) ? (44 * playerScale) : (22 * playerScale);
            bounds.Height = (currentIndexRange == forwardIndexRange || currentIndexRange == backwardIndexRange) ? (22 * playerScale) : (44 * playerScale);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState)
        {
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            // idle texture
            idleTexture = content.Load<Texture2D>("player(drawn)");
            forwardLTexture = content.Load<Texture2D>("walking (forwardL)");
            forwardRTexture = content.Load<Texture2D>("walking (forwardR)");
            playerLeftTexture = content.Load<Texture2D>("player_left");
            leftLTexture = content.Load<Texture2D>("walking (leftL)");
            leftRTexture = content.Load<Texture2D>("walking (leftR)");

            // initialize player texture
            playerTexture = idleTexture;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;

            //if (vflipped && hflipped)
            //{
            //    spriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
            //}
            playerTexture = idleTexture;

            if (moving)
            {
                vflipped = false;
                hflipped = false;

                switch (animationIndex)
                {
                    case (int)WalkingTexture.ForwardL:
                        vflipped = false;
                        playerTexture = forwardLTexture;
                        break;
                    case (int)WalkingTexture.ForwardR:
                        vflipped = false;
                        playerTexture = forwardRTexture;
                        break;
                    case (int)WalkingTexture.LeftL:
                        hflipped = false;
                        playerTexture = leftLTexture;
                        break;
                    case (int)WalkingTexture.LeftR:
                        hflipped = false;
                        playerTexture = leftRTexture;
                        break;
                    case (int)WalkingTexture.RightL:
                        hflipped = true;
                        playerTexture = leftLTexture;
                        break;
                    case (int)WalkingTexture.RightR:
                        hflipped = true;
                        playerTexture = leftRTexture;
                        break;
                    case (int)WalkingTexture.BackwardL:
                        vflipped = true;
                        playerTexture = forwardLTexture;
                        break;
                    case (int)WalkingTexture.BackwardR:
                        vflipped = true;
                        playerTexture = forwardRTexture;
                        break;
                }
            }

            if (vflipped)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            else if (hflipped)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            //// Draw bat sprite
            //var source = new Rectangle(animationIndex * 32, (int)Direction * 32, 32, 32);
            //spriteBatch.Draw(texture, Position, source, Color.White);

            //SpriteEffects spriteEffects = (vflipped) ? SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(
                playerTexture,
                playerPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                playerScale,
                spriteEffects,
                0f
            );
            //// Debug size of collider
            //var rect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            //spriteBatch.Draw(texture, rect, Color.Red);

            //spriteBatch.Draw(playerTexture, new Vector2(bounds.X, bounds.Y), Color.Red);
            //spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0, new Vector2(64, 64), 0.25f, spriteEffects, 0);
        }
    }
}
