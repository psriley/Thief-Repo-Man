using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using AutoAvenger.StateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace AutoAvenger.Screens
{
    public class CarEnemyScreen : AutoScrollScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        float timerBeforeEnemy;
        
        EnemyCar _enemyCar;
        Texture2D _enemyCarTexture;
        Texture2D _bulletTexture;

        public CarEnemyScreen() : base()
        {
            timerBeforeEnemy = 0;
        }

        // Load graphics content for the game
        public override void Activate()
        {
            base.Activate();

            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _enemyCarTexture = _content.Load<Texture2D>("forward_car");
            _bulletTexture = _content.Load<Texture2D>("bullet");
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            timerBeforeEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timerBeforeEnemy >= 10 && _enemyCar == null)
            {
                _enemyCar = new EnemyCar(_enemyCarTexture, _bulletTexture, _player);
                Debug.WriteLine("Spawning enemy...");
            }

            if (_enemyCar != null)
            {
                _enemyCar.Update(gameTime);
                _enemyCar._followerPosition.X = _player.carPosition.X;

                foreach (Bullet b in _enemyCar.bullets)
                {
                    if (b.Bounds.CollidesWith(_player.Bounds))
                    {
                        b.DamageCar(_player);
                        if (_player.health <= 0)
                        {
                            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new CarEnemyScreen());
                        }
                        b.isDestroyed = true;
                    }
                }

                foreach (Bullet b in _player.bullets)
                {
                    if (b.Bounds.CollidesWith(_enemyCar.Bounds))
                    {
                        if (b.DamageCar(_player, _enemyCar))
                        {
                            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new MainMenuScreen());
                        }
                        b.isDestroyed = true;
                    }
                }
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            base.Draw(gameTime);
            if (_enemyCar != null)
            {
                _enemyCar.Draw(spriteBatch);
            }
        }
    }
}
