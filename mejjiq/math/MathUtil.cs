using System;
using System.Collections.Generic;
using dusk.mejjiq.entities;
using Microsoft.Xna.Framework;

namespace dusk.mejjiq.math;
public static class MathUtils
{
    public static List<Vector3> GenerateCircleEdges(Vector3 center, float radius, int resolution)
    {
        List<Vector3> points = [];

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

    private static Vector3 CalculateTension(Vector3 position0, Vector3 position1, float minDistance, float tensionCoefficient){
        var direction = position1 - position0;

        float distance = direction.Length();
        float distanceRatio = minDistance/ distance;

        float distanceLog = (float)Math.Log(distanceRatio);

        var tensionVector = -direction * distanceLog / tensionCoefficient;
        return tensionVector;
    }

    public static Vector3 GetTensionVector(Edge edge, Node activeNode)
    {

        var node0 = edge.GetNodes()[0];
        var node1 = edge.GetNodes()[1];

        var position0 = node0.Position;
        var position1 = node1.Position;

        return CalculateTension(position0,position1, edge.MinLength, edge.TensionCoefficient);
    }

    public static Vector3 GetTensionVector(Vector3 position0, Vector3 position1, float minDistance, float tensionCoefficient){
        return CalculateTension(position0, position1, minDistance, tensionCoefficient);
    }

}