using AutoAvenger.Collisions;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAvenger
{
    public class EnemyCar
    {
        private BoundingRectangle _bounds;
        public BoundingRectangle Bounds => _bounds;

        private Texture2D _enemyTexture;
        private float _enemyHealth;

        public EnemyCar(Texture2D texture)
        {
            _enemyTexture = texture;
        }

        public void DamageCar(float damage)
        {
            _enemyHealth -= damage;
        }
    }
}
