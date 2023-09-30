using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Thief_Repo_Man.StateManagement;

namespace Thief_Repo_Man.Screens
{
    public class SplashScreen : GameScreen
    {
        ContentManager _content;
        Texture2D _background;
        TimeSpan _displayTime;

        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background = _content.Load<Texture2D>("city_background");
            _displayTime = TimeSpan.FromSeconds(2);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;
            if (_displayTime <= TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
