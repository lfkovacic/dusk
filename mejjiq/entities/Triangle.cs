using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using dusk.mejjiq.entities.@interface;
using dusk.mejjiq.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities;

public class Triangle
{
    public INode[] _nodes;
    private Edge[] _edges; // Represent edges as an array of IEdge
    private readonly Color[] _colors;

    private GraphicsDevice _graphicsDevice;

    public Triangle(GraphicsDevice graphicsDevice, INode node1, INode node2, INode node3)
    {
        _graphicsDevice = graphicsDevice;

        _nodes = new[] { node1, node2, node3 };
        _edges = CalculateEdges(); // Initialize edges
    }

    public Edge GetEdge(int index)
    {
        return _edges[index];
    }

    public Edge[] GetAllEdges()
    {
        return _edges;
    }

    public Triangle(GraphicsDevice graphicsDevice, INode[] nodes)
    {
        if (nodes.Length != 3)
            throw new ArgumentException("A triangle must have exactly 3 nodes.", nameof(nodes));

        _graphicsDevice = graphicsDevice;
        _nodes = nodes;
        _edges = CalculateEdges(); // Initialize edges
    }

    public void Draw(BasicEffect effect)
    {
        var green = Color.Green;

        // Use edges to define the vertices
        var vertices = _edges.SelectMany(edge =>
        {
            return new[]
            {
                new VertexPositionColor(edge.GetNodes()[0].Position, green),
                new VertexPositionColor(edge.GetNodes()[1].Position, green)
            };
        }).ToArray();

        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, _edges.Length);
            foreach (Node node in _nodes)
            {
                node.Draw(_graphicsDevice, effect);
            }
        }
    }

    public INode GetNode(int index)
    {
        if (index < 0 || index >= _nodes.Length)
            return null;

        return _nodes[index];
    }


    public Edge[] CalculateEdges()
    {
        // Create edges between the three nodes
        return
        [
            (Edge)Edge.GetEdgeFromNodes(_nodes[0], _nodes[1]),
            (Edge)Edge.GetEdgeFromNodes(_nodes[1], _nodes[2]),
            (Edge)Edge.GetEdgeFromNodes(_nodes[2], _nodes[0])
        ];
    }

    public void UpdateEdgeProperties(int index)
    {
        // Loop through all edges and update properties for the one at the specified index
        if (index < 0 || index >= _edges.Length)
        {
            throw new InvalidDataException("Invalid edge index.");
        }

        var edge = _edges[index];

        // Update tension and other properties for the edge
        // For now, we'll just update the MinLength for the edge and apply tension accordingly
        edge.MinLength = 10f; // Example MinLength, you can set this dynamically as needed

        // Apply tension (this is where you'd apply the logic for restoring equilibrium based on MinLength)


        // Additional logic for other properties like color, weight, etc. can be added here.
        // Example:
        // edge.Color = CalculateEdgeColorBasedOnTension(edge);
    }

    // ISaveable Implementation
    public JsonNode Serialize()
    {
        var jsonObject = new JsonObject
        {
            ["Nodes"] = new JsonArray(_nodes.Select(node => node.Serialize()).ToArray()),
            ["Edges"] = new JsonArray(_edges.Select(edge => edge.Serialize()).ToArray()),
            ["Colors"] = JsonUtils.ToJsonArray(_colors.Select(color => color.PackedValue))
        };
        return jsonObject;
    }

    public static Triangle Deserialize(GraphicsDevice graphicsDevice, JsonNode serializedData)
    {
        if (serializedData is not JsonObject obj)
            throw new InvalidDataException("Invalid triangle data");

        // Deserialize nodes
        Node[] deserializedNodes = new Node[3];
        if (obj["Nodes"] is JsonArray nodesArray)
        {
            for (int i = 0; i < 3; i++)
            {
                deserializedNodes[i] = (Node)Node.Deserialize(nodesArray[i]);
            }

            if (deserializedNodes.Length != 3)
                throw new InvalidDataException("Triangle must have exactly 3 nodes");
        }

        Triangle triangle = new(graphicsDevice, deserializedNodes);

        // Deserialize edges
        if (obj["Edges"] is JsonArray edgesArray)
        {
            for (int i = 0; i < edgesArray.Count; i++)
            {
                var edgeObj = edgesArray[i] as JsonObject;
                if (edgeObj == null) continue;

                int node1Index = edgeObj["Node1"]?.GetValue<int>() ?? -1;
                int node2Index = edgeObj["Node2"]?.GetValue<int>() ?? -1;

                if (node1Index >= 0 && node2Index >= 0)
                {
                    triangle._edges[i] = (Edge)Edge.GetEdgeFromNodes(
                        triangle._nodes[node1Index],
                        triangle._nodes[node2Index]
                    );
                }
            }
        }

        return triangle;
    }
}
