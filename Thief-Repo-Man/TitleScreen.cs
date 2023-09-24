using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Thief_Repo_Man
{
    public enum ArrowDirection
    {
        Up = 0,
        Down = 1,
    }

    public class TitleScreen : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D playerTexture;
        private Texture2D carTexture;
        private Texture2D notepadBackground;
        private Texture2D indicationArrow;

        private Vector2 centerScreenPosition;
        private Vector2 playerPosition;
        private Vector2 car1Position;
        private Vector2 car2Position;
        private Vector2 car3Position;
        private Vector2 movementInstructionsPosition;
        private Vector2 exitInstructionsPosition;
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

        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;

        // For debug purposes (click to see location)
        MouseState priorMouseState;
        MouseState currentMouseState;

        bool canExit = false;
        bool inCar = false;

        public TitleScreen()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerPosition = new Vector2(
                GraphicsDevice.Viewport.Width / 1.2f,
                GraphicsDevice.Viewport.Height / 4
            );
            car1Position = new Vector2(
                GraphicsDevice.Viewport.Width / 2.7f,
                GraphicsDevice.Viewport.Height / 2.25f
            );
            car2Position = new Vector2(
                GraphicsDevice.Viewport.Width / 2.7f,
                GraphicsDevice.Viewport.Height / 1.90f
            );
            car3Position = new Vector2(
                GraphicsDevice.Viewport.Width / 1.6f,
                GraphicsDevice.Viewport.Height / 2
            );
            movementInstructionsPosition = new Vector2(
                678,
                25
            );
            exitInstructionsPosition = new Vector2(
                368,
                640
            );
            //movementInstructionsPosition = new Vector2(
            //    1075,
            //    35
            //);
            indicationArrowPosition = car1Position;
            frontExitTextPosition = new Vector2(
                indicationArrowPosition.X,
                indicationArrowPosition.Y - 40f
            );
            backExitTextPosition = new Vector2(
                frontExitTextPosition.X,
                frontExitTextPosition.Y + 10f
            );

            centerScreenPosition = new Vector2(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2
            );

            //player = new CharacterController(playerPosition);
            playerInput = new PlayerInput(playerPosition);
            car1 = new Car(car1Position);

            //inputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            notepadBackground = Content.Load<Texture2D>("title_screen");
            indicationArrow = Content.Load<Texture2D>("indication_arrow");

            inkFree = Content.Load<SpriteFont>("inkFree");
            gaegu = Content.Load<SpriteFont>("gaegu");
            rubik = Content.Load<SpriteFont>("rubik");

            //player.LoadContent(Content);
            playerInput.LoadContent(Content);
            car1.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            #region Updating input state

            //priorKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            //priorGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(0);

            if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                priorMouseState.LeftButton == ButtonState.Released)
            {
                Debug.WriteLine(currentMouseState.Position);
            }

            #endregion

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

                if (canExit && currentKeyboardState.IsKeyDown(Keys.E) || currentGamePadState.Buttons.A == ButtonState.Pressed)
                {
                    //Exit();
                    playerInput.SwitchControlMode(ControlMode.Driving);
                    inCar = true;
                }
                //car1.Bounds;
                //if (car.Bounds.CollidesWith(slimeGhost.Bounds))
                //{

                //}
            }
            #endregion

            //inputManager.Update(gameTime);
            //if (inputManager.Exit) Exit();

            // TODO: Add your update logic here
            //playerPosition += inputManager.Direction;
            //playerPosition += player.Direction;

            //if (currentKeyboardState.IsKeyDown(Keys.Space))
            //{
            //    Debug.WriteLine(playerPosition);
            //}

            //player.Update(gameTime, currentKeyboardState);
            playerInput.Update(gameTime, currentKeyboardState);

            // Wrap the ship to keep it on-screen
            var viewport = GraphicsDevice.Viewport;
            if (playerPosition.Y < 0) playerPosition.Y = viewport.Height;
            if (playerPosition.Y > viewport.Height) playerPosition.Y = 0;
            if (playerPosition.X < 0) playerPosition.X = viewport.Width;
            if (playerPosition.X > viewport.Width) playerPosition.X = 0;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Color color = canExit ? Color.Red : Color.Gold;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(
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
            //_spriteBatch.Draw(
            //    playerTexture,
            //    playerPosition,
            //    null,
            //    Color.White,
            //    0f,
            //    Vector2.Zero,
            //    1.5f,
            //    SpriteEffects.None,
            //    0f
            //);
            if (!inCar)
            {
                car1.Draw(_spriteBatch);
            }
            playerInput.Draw(gameTime, _spriteBatch);
            //_spriteBatch.Draw(carTexture, car1Position, Color.White);
            //_spriteBatch.Draw(carTexture, car2Position, Color.White);
            //_spriteBatch.Draw(carTexture, car3Position, Color.White);
            //_spriteBatch.DrawString(inkFree, $"Thief Repo Man", new Vector2(2, 2), Color.Gold, 0, Vector2.Zero, 4, SpriteEffects.None, 0);
            if (!inCar)
            {
                _spriteBatch.Draw(indicationArrow, indicationArrowPosition, color);
            }
            //_spriteBatch.DrawString(gaegu, "Exit", new Vector2(car1Position.X - 6f, car1Position.Y - 6f), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(gaegu, "Exit", new Vector2(car1Position.X + 3f, car1Position.Y + 3f), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            if (!inCar)
            {
                _spriteBatch.DrawString(gaegu, "Enter", backExitTextPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                _spriteBatch.DrawString(gaegu, "Enter", frontExitTextPosition, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            _spriteBatch.DrawString(gaegu, "Instructions: Move with 'WASD',\n interact with cars using 'E'", movementInstructionsPosition, Color.Gray, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            _spriteBatch.DrawString(gaegu, "Exit the game with 'ESC'", exitInstructionsPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            _spriteBatch.DrawString(gaegu, "Exit the game with 'ESC'", new Vector2(exitInstructionsPosition.X - 4, exitInstructionsPosition.Y - 4), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(rubik, "Exit with the car!", exitInstructionsPosition, Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(inkFree, $"Exit", car2Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(inkFree, $"Options", car3Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(inkFree, $"Use 'WASD' to move and 'E' to repo vehicles", new Vector2(GraphicsDevice.Viewport.Width / 7, GraphicsDevice.Viewport.Height / 1.1f), Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}