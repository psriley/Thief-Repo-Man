using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using Thief_Repo_Man.ParticleSystem;

namespace Thief_Repo_Man.Particle_System.Systems
{
    public class SmokeParticleSystem : ParticleSystem
    {
        Color[] colors = new Color[]
        {
            Color.DarkGray,
            Color.LightGray,
            Color.White,
        };

        IParticleEmitter _emitter;

        ///// <summary>
        ///// We only want the smoke to appear if the car is not moving (idling).
        ///// </summary>
        public bool IsIdling { get; set; } = true;

        public SmokeParticleSystem(Game game, IParticleEmitter emitter) : base(game, 500)
        {
            _emitter = emitter;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "circle";

            minNumParticles = 1;
            maxNumParticles = 3;

            blendState = BlendState.AlphaBlend;
            DrawOrder = AlphaBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = _emitter.Velocity;

            var rotation = _emitter.Rotation;

            var acceleration = new Vector2(RandomHelper.NextFloat(0.5f, 1f), RandomHelper.NextFloat(-0.5f, 0.5f)) * 75;

            var scale = RandomHelper.NextFloat(0.5f, 1);

            var lifetime = RandomHelper.NextFloat(0.1f, 1.0f);

            Vector2 offset = new Vector2(50, -10);

            p.Initialize(where + offset, velocity, acceleration, colors[RandomHelper.Next(colors.Length)], rotation: rotation, scale: scale, lifetime: lifetime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsIdling) AddParticles(_emitter.Position);
        }

        //Rectangle _source;

        ///// <summary>
        ///// We only want the smoke to appear if the car is not moving (idling).
        ///// </summary>
        //public bool IsIdling { get; set; } = true;

        //public SmokeParticleSystem(Game game, Rectangle source) : base(game, 20)
        //{
        //    _source = source;
        //}

        //protected override void InitializeConstants()
        //{
        //    textureFilename = "circle";
        //    minNumParticles = 5;
        //    maxNumParticles = 20;
        //}

        //protected override void InitializeParticle(ref Particle p, Vector2 where)
        //{
        //    p.Initialize(where, -Vector2.UnitY * 20, -Vector2.UnitY * 2, Color.DarkGray, scale: 0.5f, lifetime: 0.5f);
        //}

        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);

        //    if(IsIdling) AddParticles(_source);
        //}
    }
}
