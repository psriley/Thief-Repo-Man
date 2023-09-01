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

    public class Game1 : Game
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
        private Player player;
        private Car car1;
        private Car car2;
        private Car exitCar;

        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;

        // For debug purposes (click to see location)
        MouseState priorMouseState;
        MouseState currentMouseState;

        bool canExit = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
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
                1075,
                35
            );
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

            player = new Player(playerPosition);
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

            player.LoadContent(Content);
            car1.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(0);

            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                priorMouseState.LeftButton == ButtonState.Released)
            {
                Debug.WriteLine(currentMouseState.Position);
            }

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
            if (car1.Bounds.CollidesWith(player.Bounds))
            {
                if (!canExit) { canExit = true; }
            }
            else
            {
                if (canExit) { canExit = false; }
            }

            if (canExit && currentKeyboardState.IsKeyDown(Keys.E) || currentGamePadState.Buttons.A == ButtonState.Pressed)
            {
                Exit();
            }
            //car1.Bounds;
            //if (car.Bounds.CollidesWith(slimeGhost.Bounds))
            //{

            //}
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
            player.Update(gameTime);

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
            car1.Draw(_spriteBatch);
            player.Draw(gameTime, _spriteBatch);
            //_spriteBatch.Draw(carTexture, car1Position, Color.White);
            //_spriteBatch.Draw(carTexture, car2Position, Color.White);
            //_spriteBatch.Draw(carTexture, car3Position, Color.White);
            //_spriteBatch.DrawString(inkFree, $"Thief Repo Man", new Vector2(2, 2), Color.Gold, 0, Vector2.Zero, 4, SpriteEffects.None, 0);
            _spriteBatch.Draw(indicationArrow, indicationArrowPosition, color);
            //_spriteBatch.DrawString(gaegu, "Exit", new Vector2(car1Position.X - 6f, car1Position.Y - 6f), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(gaegu, "Exit", new Vector2(car1Position.X + 3f, car1Position.Y + 3f), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            _spriteBatch.DrawString(gaegu, "Exit", backExitTextPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            _spriteBatch.DrawString(gaegu, "Exit", frontExitTextPosition, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            _spriteBatch.DrawString(gaegu, "Instructions: Move with 'WASD',\n interact with cars using 'E'", movementInstructionsPosition, Color.Gray, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(rubik, "Exit with the car!", exitInstructionsPosition, Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(inkFree, $"Exit", car2Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(inkFree, $"Options", car3Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            //_spriteBatch.DrawString(inkFree, $"Use 'WASD' to move and 'E' to repo vehicles", new Vector2(GraphicsDevice.Viewport.Width / 7, GraphicsDevice.Viewport.Height / 1.1f), Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}