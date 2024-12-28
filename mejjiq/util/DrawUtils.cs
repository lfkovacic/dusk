using System.Collections.Generic;
using dusk.mejjiq.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.util
{
    public static class DrawUtils
    {
        public static void DrawCircle(GraphicsDevice graphicsDevice, BasicEffect effect, Vector3 origin, float radius, Color color)
        {
            List<Vector3> circleEdges = MathUtils.GenerateCircleEdges(origin, radius, 50);
            VertexPositionColor[] vertices = new VertexPositionColor[circleEdges.Count * 2];
            for (int i = 0; i < circleEdges.Count; i++)
            {
                // Connect the current point to the next point (looping back to 0 after the last point)
                int nextIndex = (i + 1) % circleEdges.Count;

                // Create the line between two points (current and next point)
                vertices[i * 2] = new VertexPositionColor(circleEdges[i], color);
                vertices[i * 2 + 1] = new VertexPositionColor(circleEdges[nextIndex], color);
                foreach (var pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, circleEdges.Count);
                }
            }
        }

    }
}