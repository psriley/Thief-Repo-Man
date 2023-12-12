using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAvenger
{
    public class ObstacleGenerator
    {
        public List<Obstacle> obstacleList = new();

        private int _minObstaclesPerBackground;
        private int _maxObstaclesPerBackground;
        private bool _isActive;
        private Texture2D _obstacleTexture;
        private Texture2D _debugTexture;
        private Texture2D _damagedTexture;
        private List<ScrollingBackground> _scrollingBackgrounds = new();

        // This variable is the number of scrolling backgrounds that have passed since the oldest obstacles still currently active and
        // moving downwards, were placed. This allows the obstacles to be sufficiently offscreen before being repositioned.
        private int _backgroundsPassed;

        public ObstacleGenerator(Texture2D obstacleTexture, Texture2D debugTexture, Texture2D damagedTexture, int minObstaclesPerBackground, int maxObstaclesPerBackground, bool isActive, List<ScrollingBackground> scrollingBackgrounds) 
        {
            _obstacleTexture = obstacleTexture;
            _debugTexture = debugTexture;
            _damagedTexture = damagedTexture;
            _minObstaclesPerBackground = minObstaclesPerBackground;
            _maxObstaclesPerBackground = maxObstaclesPerBackground;
            _isActive = isActive;
            _scrollingBackgrounds = scrollingBackgrounds;

            // TODO: COULD BE WORTH CHANGING THIS LATER BECAUSE THIS ASSUMES THERE ARE ONLY 2 SCROLLING BACKGROUNDS!
            if (scrollingBackgrounds.Count != 2)
            {
                throw new ArgumentException("This constructor assumes there are only 2 scrolling backgrounds. It will need refactoring to accommodate a different number.");
            }

            _backgroundsPassed = 0;

            //Generate(_scrollingBackgrounds.Last());
        }

        public void Generate(ScrollingBackground backgroundToAddTo)
        {
            if (_isActive)
            {
                // Check if we need to move any previously created obstacles to new background from off screen background
                if (obstacleList.Count > 0 && _backgroundsPassed >= 2)
                {
                    MoveOffScreenObstacles(backgroundToAddTo);
                    _backgroundsPassed = 0;
                }
                if (_backgroundsPassed < 2)
                {
                    CreateObstacles(backgroundToAddTo);
                }

                // Because 'Generate' is called every time the scrolling background is moved, this also means it is a good candidate to
                // increment this variable.
                _backgroundsPassed++;
            }
        }

        public void GenerateEnemy(ScrollingBackground backgroundToAddTo)
        {
            if (_isActive)
            {

            }
        }

        private void CreateObstacles(ScrollingBackground background)
        {
            for (int i = obstacleList.Count; i <= _maxObstaclesPerBackground; i++)
            {
                Obstacle obstacle = new Obstacle(background, _obstacleTexture, _debugTexture, _damagedTexture);
                obstacleList.Add(obstacle);
                Debug.WriteLine($"Obstacle: {obstacle}, Position: {obstacle.position}");
            }
        }

        private void MoveOffScreenObstacles(ScrollingBackground background)
        {
            Random rand = new();

            foreach (Obstacle o in obstacleList)
            {
                o.position = new Vector2(rand.Next(background.backgroundRect.Left, background.backgroundRect.Right), rand.Next(background.backgroundRect.Top, background.backgroundRect.Bottom));
                o.bounds.X = o.position.X;
                o.bounds.Y = o.position.Y;
                o.isDestroyed = false;
            }
        }
    }
}
