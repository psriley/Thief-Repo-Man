using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using AutoAvenger.Particle_System.Systems;
using AutoAvenger.StateManagement;

namespace AutoAvenger.Screens
{
    public enum ArrowDirection
    {
        Up = 0,
        Down = 1,
    }

    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class TitleScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        /// <summary>
        /// Vector from the player to the origin of the screen.
        /// </summary>
        private Vector2 _playerOffset = new Vector2(-100, 100);

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        //movementInstructionsPosition = new Vector2(
        //    1075,
        //    35
        //);

        #region Title Screen specific

        private Texture2D playerTexture;
        private Texture2D carTexture;
        private Texture2D notepadBackground;
        private Texture2D indicationArrow;

        private Vector2 centerScreenPosition = new Vector2(640, 360);
        private Vector2 playerPosition = new Vector2(1066, 180);
        private Vector2 car1Position = new Vector2(474, 320);
        //private Vector2 car2Position = new Vector2(474, 378);
        //private Vector2 car3Position = new Vector2(984, 360);
        private Vector2 movementInstructionsPosition = new Vector2(678, 25);
        private Vector2 exitInstructionsPosition = new Vector2(368, 640);
        private Vector2 indicationArrowPosition;
        private Vector2 backExitTextPosition;
        private Vector2 frontExitTextPosition;

        private ArrowDirection arrowDirection;
        private double directionTimer;

        //private InputManager inputManager;
        private SpriteFont inkFree;
        private SpriteFont gaegu;
        private SpriteFont rubik;
        private PlayerInput playerInput;
        private Car car1;
        private Car car2;
        private Car exitCar;
        private SoundEffect carStart;
        private Song backgroundMusic;

        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;

        // For debug purposes (click to see location)
        MouseState priorMouseState;
        MouseState currentMouseState;

        bool canExit = false;
        bool inCar = false;

        #endregion

        public TitleScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gaegu");

            // TODO: use this.Content to load your game content here
            notepadBackground = _content.Load<Texture2D>("title_screen");
            indicationArrow = _content.Load<Texture2D>("indication_arrow");

            inkFree = _content.Load<SpriteFont>("inkFree");
            gaegu = _content.Load<SpriteFont>("gaegu");
            rubik = _content.Load<SpriteFont>("rubik");
            carStart = _content.Load<SoundEffect>("car_start");

            indicationArrowPosition = car1Position;
            frontExitTextPosition = new Vector2(
                indicationArrowPosition.X,
                indicationArrowPosition.Y - 40f
            );
            backExitTextPosition = new Vector2(
                frontExitTextPosition.X,
                frontExitTextPosition.Y + 10f
            );

            //player = new CharacterController(playerPosition);
            playerInput = new PlayerInput(playerPosition);
            car1 = new Car(car1Position, true);

            //player.LoadContent(Content);
            playerInput.LoadContent(_content);
            car1.LoadContent(_content);

            backgroundMusic = _content.Load<Song>("moving_slowly");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            SmokeParticleSystem smoke = new SmokeParticleSystem(ScreenManager.Game, playerInput.carController);

            ScreenManager.Game.Components.Add(smoke);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                #region Handling indication direction
                // Update the direction timer
                directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // Switch directions every 2 seconds
                if (directionTimer > 2.0)
                {
                    switch (arrowDirection)
                    {
                        case ArrowDirection.Up:
                            arrowDirection = ArrowDirection.Down;
                            break;
                        case ArrowDirection.Down:
                            arrowDirection = ArrowDirection.Up;
                            break;
                    }
                    directionTimer -= 2.0;
                }

                // Move the indication arrow up and down
                switch (arrowDirection)
                {
                    case ArrowDirection.Up:
                        indicationArrowPosition += new Vector2(0, -0.25f) * 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case ArrowDirection.Down:
                        indicationArrowPosition += new Vector2(0, 0.25f) * 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                }
                frontExitTextPosition = new Vector2(indicationArrowPosition.X, indicationArrowPosition.Y - 40f);
                backExitTextPosition = new Vector2(frontExitTextPosition.X + 3f, frontExitTextPosition.Y + 3f);
                #endregion
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            playerInput.HandleInput(gameTime, keyboardState);

            currentMouseState = Mouse.GetState();

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                //if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                //{
                //    ExitScreen();
                //}

                if (currentMouseState.LeftButton == ButtonState.Pressed &&
                    priorMouseState.LeftButton == ButtonState.Released)
                {
                    Debug.WriteLine(currentMouseState.Position);
                }

                if (keyboardState.IsKeyDown(Keys.P)) 
                {
                    playerInput.Score++;
                }

                #region Handling collision

                // Calculate collisions if the currentMode of the player is walking.
                if (playerInput.currentMode == 0)
                {
                    // The characterController will only exist here if the currentMode is walking.
                    if (car1.Bounds.CollidesWith(playerInput.characterController.Bounds))
                    {
                        if (!canExit) { canExit = true; }
                    }
                    else
                    {
                        if (canExit) { canExit = false; }
                    }

                    if (canExit && keyboardState.IsKeyDown(Keys.E) || gamePadState.Buttons.A == ButtonState.Pressed)
                    {
                        playerInput.carController.SetCarToControl(car1);
                        //playerInput.carController.car = car1;
                        //Exit();
                        // TODO: play car ignition sound effect
                        carStart.Play();
                        playerInput.SwitchControlMode(ControlMode.Driving);
                        inCar = true;
                    }
                    //car1.Bounds;
                    //if (car.Bounds.CollidesWith(slimeGhost.Bounds))
                    //{

                    //}
                }
                #endregion
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix transform = Matrix.Identity;
            if (inCar)
            {
                // Player-synced scrolling
                Vector2 offset = new Vector2(640 - playerInput.carController.Position.X, 360 - playerInput.carController.Position.Y);

                // Create the translation matrix representing the offset
                transform = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            }

            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            Color color = canExit ? Color.Red : Color.Gold;
                
            // Draw the transformed game world
            spriteBatch.Begin(transformMatrix: transform);

            //spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
            //spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
            //                       _enemyPosition, Color.DarkRed);


            spriteBatch.Draw(
                notepadBackground,
                centerScreenPosition,
                null,
                Color.White,
                0f,
                new Vector2(notepadBackground.Width / 2, notepadBackground.Height / 2),
                1f,
                SpriteEffects.None,
                0f
            );
            if (!inCar)
            {
                car1.Draw(spriteBatch);
            }
            playerInput.Draw(gameTime, spriteBatch);
            if (!inCar)
            {
                spriteBatch.Draw(indicationArrow, indicationArrowPosition, color);
            }
            if (!inCar)
            {
                spriteBatch.DrawString(gaegu, "Enter", backExitTextPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(gaegu, "Enter", frontExitTextPosition, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            spriteBatch.DrawString(gaegu, "Instructions: Move with 'WASD',\n interact with cars using 'E'", movementInstructionsPosition, Color.Gray, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(gaegu, "Exit the game with 'ESC'", exitInstructionsPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(gaegu, "Exit the game with 'ESC'", new Vector2(exitInstructionsPosition.X - 4, exitInstructionsPosition.Y - 4), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(gaegu, $"Score: {playerInput.Score}", Vector2.Zero, Color.Purple);


            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
