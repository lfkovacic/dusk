using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace dusk.mejjiq.math;
public static class MathUtils 
{
     public static List<Vector3> GenerateCircleEdges(Vector3 center, float radius, int resolution)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < resolution; i++)
        {
            // Calculate angle for the current point
            float angle = MathHelper.TwoPi * i / resolution;

            // Calculate the position of the point on the circle
            float x = center.X + radius * (float)Math.Cos(angle);
            float y = center.Y + radius * (float)Math.Sin(angle);

            // Add the point to the list
            points.Add(new Vector3(x, y, center.Z)); // Assuming the circle is flat on the Z-plane
        }

        return points;
    }
    

}