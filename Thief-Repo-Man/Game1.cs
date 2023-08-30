using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Thief_Repo_Man
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D playerTexture;
        private Texture2D carTexture;
        private Vector2 playerPosition;

        private Vector2 car1Position;
        private Vector2 car2Position;
        private Vector2 car3Position;

        private InputManager inputManager;
        private SpriteFont inkFree;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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
                GraphicsDevice.Viewport.Width / 4,
                GraphicsDevice.Viewport.Height / 3.75f
            );
            car2Position = new Vector2(
                GraphicsDevice.Viewport.Width / 4,
                GraphicsDevice.Viewport.Height / 1.5f
            );
            car3Position = new Vector2(
                GraphicsDevice.Viewport.Width / 1.6f,
                GraphicsDevice.Viewport.Height / 2
            );

            inputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("player(drawn)");
            carTexture = Content.Load<Texture2D>("car");

            inkFree = Content.Load<SpriteFont>("inkFree");
        }

        protected override void Update(GameTime gameTime)
        {
            inputManager.Update(gameTime);
            if (inputManager.Exit) Exit();

            // TODO: Add your update logic here
            playerPosition += inputManager.Direction;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerTexture, playerPosition, Color.White);
            _spriteBatch.Draw(carTexture, car1Position, Color.White);
            _spriteBatch.Draw(carTexture, car2Position, Color.White);
            _spriteBatch.Draw(carTexture, car3Position, Color.White);
            _spriteBatch.DrawString(inkFree, $"Thief Repo Man", new Vector2(2, 2), Color.Gold, 0, Vector2.Zero, 4, SpriteEffects.None, 0);
            _spriteBatch.DrawString(inkFree, $"Play", car1Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.DrawString(inkFree, $"Exit", car2Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.DrawString(inkFree, $"Options", car3Position, Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.DrawString(inkFree, $"Use 'WASD' to move and 'E' to repo vehicles", new Vector2(GraphicsDevice.Viewport.Width / 7, GraphicsDevice.Viewport.Height / 1.1f), Color.Gold, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}