using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json.Nodes;
using dusk.mejjiq.entities.@interface;
using dusk.mejjiq.math;

namespace dusk.mejjiq.entities;

public class Edge : IEdge
{
    public INode[] Nodes { get; set; } = new INode[2];
    public float MinLength { get; set; } = 200f; // Minimum length where tension starts to approach zero
    public float TensionCoefficient = 2f; //The higher this valiue, the less springy
    public Edge(INode node0, INode node1)
    {
        Nodes[0] = node0;
        Nodes[1] = node1;
    }

    public Edge(INode node0, INode node1, float minLength)
    {
        Nodes[0] = node0;
        Nodes[1] = node1;
        MinLength = minLength;
    }

    public static IEdge GetEdgeFromNodes(INode node0, INode node1)
    {
        return new Edge(node0, node1);
    }

    // Apply the tension with a logarithmic scaling
    public void ApplyTension(Node activeNode)
    {
        MathUtils.ApplyTension(this, activeNode);
    }



    public Vector3 ToVector3(IEdge v)
    {
        if (v is Edge edge)
        {
            // Calculate the direction vector from the first node to the second node
            var position0 = edge.Nodes[0].Position;
            var position1 = edge.Nodes[1].Position;

            return new Vector3(
                position1.X - position0.X,
                position1.Y - position0.Y,
                position1.Z - position0.Z
            );
        }

        // Return a zero vector as a fallback (or throw an exception if unexpected)
        throw new InvalidDataException("Unexpected type of edge.");
    }
    public JsonNode Serialize()
    {
        var jsonObject = new JsonObject
        {
            ["Node1"] = Nodes[0].ID, // Assuming nodes have unique IDs
            ["Node2"] = Nodes[1].ID // Use IDs to reference nodes
        };

        // In the future, add properties like "Tension" or "Weight" if needed
        return jsonObject;
    }

    // Deserialize the edge
    public static IEdge Deserialize(JsonNode serializedData, INode[] nodes)
    {
        if (serializedData is not JsonObject obj)
            throw new InvalidDataException("Invalid edge data");

        int node1Id = obj["Node1"]?.GetValue<int>() ?? throw new InvalidDataException("Missing Node1 ID");
        int node2Id = obj["Node2"]?.GetValue<int>() ?? throw new InvalidDataException("Missing Node2 ID");

        // Find the actual node instances by their IDs
        INode node1 = nodes.FirstOrDefault(node => node.ID == node1Id);
        INode node2 = nodes.FirstOrDefault(node => node.ID == node2Id);

        if (node1 == null || node2 == null)
            throw new InvalidDataException("Invalid node references in edge");

        return new Edge(node1, node2);
    }

    public INode[] GetNodes()
    {
        return Nodes;
    }
}