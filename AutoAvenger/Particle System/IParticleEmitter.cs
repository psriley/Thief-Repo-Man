using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AutoAvenger.Particle_System
{
    public interface IParticleEmitter
    {
        public Vector2 EmitterPosition { get; }
        
        public Vector2 EmitterVelocity { get; }

        public float EmitterRotation { get; }
    }
}
