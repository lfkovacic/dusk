using System;
using System.Collections.Generic;
using dusk.mejjiq.entities;
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

    public static void ApplyTension(Edge edge, Node activeNode)
    {

        var node0 = edge.GetNodes()[0];
        var node1 = edge.GetNodes()[1];

        // if (node1 == activeNode) return;

        var position0 = node0.Position;
        var position1 = node1.Position;

        var direction = position1 - position0;

        float distance = direction.Length();
        float distanceRatio = edge.MinLength / distance;

        float distanceLog = (float)Math.Log(distanceRatio);
        ((Node)node0).MoveByVector(-direction * distanceLog / edge.TensionCoefficient);
        ((Node)node1).MoveByVector(direction * distanceLog / edge.TensionCoefficient);
    }
}