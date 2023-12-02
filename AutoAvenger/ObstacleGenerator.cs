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

        private int _minObstaclesOnScreen;
        private int _maxObstaclesOnScreen;
        private bool _isActive;
        private Texture2D _obstacleTexture;

        public ObstacleGenerator(Texture2D obstacleTexture, int minObstaclesOnScreen, int maxObstaclesOnScreen, bool isActive) 
        {
            _obstacleTexture = obstacleTexture;
            _minObstaclesOnScreen = minObstaclesOnScreen;
            _maxObstaclesOnScreen = maxObstaclesOnScreen;
            _isActive = isActive;
        }

        public void Generate(ScrollingBackground backgroundToAddTo)
        {
            if (_isActive)
            {
                // Check if we need to move any previously created obstacles to new background from off screen background
                if (obstacleList.Count > 0)
                {
                    MoveOffScreenObstacles(backgroundToAddTo);
                }
                if (obstacleList.Count < _maxObstaclesOnScreen)
                {
                    CreateObstacles(backgroundToAddTo);
                }
            }
        }

        private void CreateObstacles(ScrollingBackground background)
        {
            if (obstacleList.Count < _maxObstaclesOnScreen)
            {
                for (int i = obstacleList.Count; i <= _maxObstaclesOnScreen; i++)
                {
                    Obstacle obstacle = new Obstacle(background, _obstacleTexture);
                    obstacleList.Add(obstacle);
                    Debug.WriteLine($"Obstacle: {obstacle}, Position: {obstacle.position}");
                }
            }
        }

        private void MoveOffScreenObstacles(ScrollingBackground background)
        {
            Random rand = new();

            foreach (Obstacle o in obstacleList)
            {
                o.position = new Vector2(rand.Next(0, background.backgroundRect.Width), rand.Next(0, background.backgroundRect.Height));
            }
        }
    }
}
