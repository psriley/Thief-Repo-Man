﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AutoAvenger.StateManagement;

namespace AutoAvenger.Screens
{
    // Helper class represents a single entry in a MenuScreen. By default this
    // just draws the entry text string, but it can be customized to display menu
    // entries in different ways. This also provides an event that will be raised
    // when the menu entry is selected.
    public class MenuEntry
    {
        private string _text;
        private float _selectionFade;    // Entries transition out of the selection effect when they are deselected
        private Vector2 _position;    // This is set by the MenuScreen each frame in Update

        public string Text
        {
            private get => _text;
            set => _text = value;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(string text)
        {
            _text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }


        // This can be overridden to customize the appearance.
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * _selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, _text, _position, color, 0,
                origin, scale, SpriteEffects.None, 0);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
