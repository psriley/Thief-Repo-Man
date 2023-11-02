using AutoAvenger.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoAvenger.Screens
{
    //public class Test3DScreen : GameScreen
    //{
    //    private ContentManager _content;
    //    private SpriteFont _gameFont;

    //    private float _pauseAlpha;
    //    private readonly InputAction _pauseAction;
    //    //private PlayerInput _playerInput;

    //    // For debug purposes (click to see location)
    //    MouseState priorMouseState;
    //    MouseState currentMouseState;

    //    // The cube to draw
    //    Cube cube;

    //    public Test3DScreen()
    //    {
    //        TransitionOnTime = TimeSpan.FromSeconds(1.5);
    //        TransitionOffTime = TimeSpan.FromSeconds(0.5);

    //        _pauseAction = new InputAction(
    //            new[] { Buttons.Start, Buttons.Back },
    //            new[] { Keys.Back, Keys.Escape }, true);
    //    }

    //    // Load graphics content for the game
    //    public override void Activate()
    //    {
    //        if (_content == null)
    //            _content = new ContentManager(ScreenManager.Game.Services, "Content");

    //        cube = new Cube(ScreenManager.Game);

    //        //_playerInput = new PlayerInput(playerPosition);

    //        // A real game would probably have more content than this sample, so
    //        // it would take longer to load. We simulate that by delaying for a
    //        // while, giving you a chance to admire the beautiful loading screen.
    //        Thread.Sleep(1000);

    //        // once the load has finished, we use ResetElapsedTime to tell the game's
    //        // timing mechanism that we have just finished a very long frame, and that
    //        // it should not try to catch up.
    //        ScreenManager.Game.ResetElapsedTime();
    //    }

    //    // This method checks the GameScreen.IsActive property, so the game will
    //    // stop updating when the pause menu is active, or if you tab away to a different application.
    //    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    //    {
    //        base.Update(gameTime, otherScreenHasFocus, false);

    //        // Gradually fade in or out depending on whether we are covered by the pause screen.
    //        if (coveredByOtherScreen)
    //            _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
    //        else
    //            _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

    //        if (IsActive)
    //        {
    //            // Put code that you want to happen on update for the scene here
    //            cube.Update(gameTime);
    //        }
    //    }

    //    // Unlike the Update method, this will only be called when the gameplay screen is active.
    //    public override void HandleInput(GameTime gameTime, InputState input)
    //    {
    //        if (input == null)
    //            throw new ArgumentNullException(nameof(input));

    //        // Look up inputs for the active player profile.
    //        int playerIndex = (int)ControllingPlayer.Value;

    //        var keyboardState = input.CurrentKeyboardStates[playerIndex];
    //        var gamePadState = input.CurrentGamePadStates[playerIndex];

    //        //playerInput.HandleInput(gameTime, keyboardState);

    //        currentMouseState = Mouse.GetState();

    //        // The game pauses either if the user presses the pause button, or if
    //        // they unplug the active gamepad. This requires us to keep track of
    //        // whether a gamepad was ever plugged in, because we don't want to pause
    //        // on PC if they are playing with a keyboard and have no gamepad at all!
    //        bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

    //        PlayerIndex player;
    //        if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
    //        {
    //            ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
    //        }
    //        else
    //        {
    //            if (currentMouseState.LeftButton == ButtonState.Pressed &&
    //                priorMouseState.LeftButton == ButtonState.Released)
    //            {
    //                Debug.WriteLine(currentMouseState.Position);
    //            }
    //        }
    //    }

    //    public override void Draw(GameTime gameTime)
    //    {
    //        // This game has a blue background. Why? Because!
    //        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

    //        // Our player and enemy are both actually just text strings.
    //        var spriteBatch = ScreenManager.SpriteBatch;

    //        // Draw the transformed game world
    //        spriteBatch.Begin();
    //        // Put spriteBatch.Draw() calls here
    //        cube.Draw();
    //        spriteBatch.End();

    //        // If the game is transitioning on or off, fade it out to black.
    //        if (TransitionPosition > 0 || _pauseAlpha > 0)
    //        {
    //            float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

    //            ScreenManager.FadeBackBufferToBlack(alpha);
    //        }
    //    }
    //}

    public class Test3DScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        //private PlayerInput _playerInput;

        // For debug purposes (click to see location)
        MouseState priorMouseState;
        MouseState currentMouseState;

        //Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        // The model to draw
        Model model;

        public Test3DScreen()
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

            //Setup Camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -5);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), ScreenManager.GraphicsDevice
                               .Viewport.AspectRatio,
                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.
                          Forward, Vector3.Up);
            model = _content.Load<Model>("Box");

            //_playerInput = new PlayerInput(playerPosition);

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
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
                // Put code that you want to happen on update for the scene here
                //cube.Update(gameTime);

                // Orbit model
                Matrix rotationMatrix = Matrix.CreateRotationY(
                                        MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition,
                              rotationMatrix);
                viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
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

            //playerInput.HandleInput(gameTime, keyboardState);

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
                if (currentMouseState.LeftButton == ButtonState.Pressed &&
                    priorMouseState.LeftButton == ButtonState.Released)
                {
                    Debug.WriteLine(currentMouseState.Position);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            // Draw the transformed game world
            spriteBatch.Begin();
            // Put spriteBatch.Draw() calls here
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
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
