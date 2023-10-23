using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace AutoAvenger
{
    public enum ControlMode
    {
        Walking,
        Driving
    }

    public class PlayerInput
    {
        //KeyboardState currentKeyboardState;
        //KeyboardState priorKeyboardState;
        //MouseState currentMouseState;
        //MouseState priorMouseState;
        //GamePadState currentGamePadState;
        //GamePadState priorGamePadState;

        private static Vector2 _direction;
        /// <summary>
        /// Direction that the player is facing.
        /// </summary>
        public static Vector2 Direction => _direction;

        /// <summary>
        /// Indicates whether the player is moving or not.
        /// </summary>
        public static bool Moving => _direction != Vector2.Zero;

        public ControlMode currentMode;

        public CharacterController characterController;
        public CarController carController;

        public PlayerInput(Vector2 position)
        {
            currentMode = ControlMode.Walking; // start in walking mode
            characterController = new CharacterController(position);
            carController = new CarController(position);
        }

        /// <summary>
        /// Switch the control mode of the player (walking or driving).
        /// </summary>
        /// <param name="newMode"></param>
        public void SwitchControlMode(ControlMode newMode)
        {
            currentMode = newMode;
        }

        public void HandleInput(GameTime gt, KeyboardState ks)
        {
            Update(gt, ks);
        }

        /// <summary>
        /// Set the direction based off of the keys that the player presses.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            switch (currentMode)
            {
                case ControlMode.Walking:
                    characterController.HandleInput(gameTime, currentKeyboardState);
                    break;
                case ControlMode.Driving:
                    carController.HandleInput(gameTime, currentKeyboardState);
                    break;
            }
        }

        public void LoadContent(ContentManager content)
        {
            characterController.LoadContent(content);
            carController.LoadContent(content);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (currentMode)
            {
                case ControlMode.Walking:
                    characterController.Draw(gameTime, spriteBatch);
                    break;
                case ControlMode.Driving:
                    carController.Draw(gameTime, spriteBatch);
                    break;
            }
        }
    }
}
