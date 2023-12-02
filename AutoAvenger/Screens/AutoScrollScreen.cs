using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAvenger.StateManagement;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace AutoAvenger.Screens
{
    public class AutoScrollScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        ScrollingBackground _scrollingBackground1;
        ScrollingBackground _scrollingBackground2;
        SimpleAutoScrollPlayer _player;
        Vector2 _playerCarPosition;
        ObstacleGenerator _obsGenerator;

        MouseState currentMouseState;

        public AutoScrollScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _playerCarPosition = new Vector2(1280 / 2, 720 / 1.2f);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _scrollingBackground1 = new ScrollingBackground(_content.Load<Texture2D>("scrolling_background"), new Rectangle(0, 0, 1280, 720));
            _scrollingBackground2 = new ScrollingBackground(_content.Load<Texture2D>("scrolling_background"), new Rectangle(0, -720, 1280, 720));

            _player = new SimpleAutoScrollPlayer(_playerCarPosition);
            _player.LoadContent(_content);

            // Obstacles
            _obsGenerator = new(_content.Load<Texture2D>("spike_trap"), 2, 5, true, new List<ScrollingBackground> { _scrollingBackground1, _scrollingBackground2 });
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
                if (_obsGenerator.obstacleList.Count > 0)
                {
                    foreach (Obstacle o in _obsGenerator.obstacleList)
                    {
                        o.Update(gameTime, _scrollingBackground1.backgroundSpeed);
                    }
                }

                if (_scrollingBackground1.backgroundRect.Y - _scrollingBackground1.backgroundTexture.Height >= 0)
                {
                    _scrollingBackground1.backgroundRect.Y = _scrollingBackground2.backgroundRect.Y - _scrollingBackground2.backgroundTexture.Height;
                    _obsGenerator.Generate(_scrollingBackground1);
                }
                if (_scrollingBackground2.backgroundRect.Y - _scrollingBackground2.backgroundTexture.Height >= 0)
                {
                    _scrollingBackground2.backgroundRect.Y = _scrollingBackground1.backgroundRect.Y - _scrollingBackground1.backgroundTexture.Height;
                    _obsGenerator.Generate(_scrollingBackground2);
                }

                _scrollingBackground1.Update(gameTime);
                _scrollingBackground2.Update(gameTime);
            }
        }

        private void GenerateObstacles()
        {

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
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    Debug.WriteLine($"Player Position: {_player.carPosition}");
                }
                _player.HandleInput(gameTime, keyboardState);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            _scrollingBackground1.Draw(spriteBatch);
            _scrollingBackground2.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            if (_obsGenerator.obstacleList != null)
            {
                foreach (Obstacle o in _obsGenerator.obstacleList)
                {
                    o.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}
