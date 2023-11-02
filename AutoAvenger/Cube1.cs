//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AutoAvenger
//{
//    public class Cube1
//    {
//        /// <summary>
//        /// The game this cube belongs to 
//        /// </summary>
//        Game game;

//        Model model;

//        /// <summary>
//        /// Constructs a cube instance
//        /// </summary>
//        /// <param name="game">The game that is creating the cube</param>
//        public Cube1(Game game)
//        {
//            this.game = game;
//        }

//        /// <summary>
//        /// Updates the Cube
//        /// </summary>
//        /// <param name="gameTime"></param>
//        public void Update(GameTime gameTime)
//        {
//            // speed from 0 to 1 with 1 being realtime and 0.5 being half as fast.
//            float speedFactor = 0.2f; 
//            float angle = (float)gameTime.TotalGameTime.TotalSeconds * speedFactor;
//            // Look at the cube from farther away while spinning around it
//            effect.View = Matrix.CreateRotationY(angle) * Matrix.CreateLookAt(
//                new Vector3(0, 0, -20),
//                new Vector3(5, 0, 0),
//                Vector3.Up
//            );
//        }

//        /// <summary>
//        /// Draws the Cube
//        /// </summary>
//        public void Draw()
//        {
//            // apply the effect 
//            effect.CurrentTechnique.Passes[0].Apply();
//            // set the vertex buffer
//            game.GraphicsDevice.SetVertexBuffer(vertices);
//            // set the index buffer
//            game.GraphicsDevice.Indices = indices;
//            // Draw the triangles
//            game.GraphicsDevice.DrawIndexedPrimitives(
//                PrimitiveType.TriangleList, // Tye type to draw
//                0,                          // The first vertex to use
//                0,                          // The first index to use
//                12                          // the number of triangles to draw
//            );
//        }
//    }
//}
