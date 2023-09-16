using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Thief_Repo_Man
{
    public class PlayerInput
    {
        private static Vector2 _direction;
        /// <summary>
        /// Direction that the player is facing.
        /// </summary>
        public static Vector2 Direction => _direction;

        /// <summary>
        /// Indicates whether the player is moving or not.
        /// </summary>
        public static bool Moving => _direction != Vector2.Zero;

        /// <summary>
        /// Set the direction based off of the keys that the player presses.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _direction = Vector2.Zero;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.GetPressedKeyCount() > 0)
            {
                if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
                if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
                if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;
                if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;
            }
        }
    }
}
