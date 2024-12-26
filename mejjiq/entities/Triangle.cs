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
    private readonly Color[] _colors;

    private GraphicsDevice _graphicsDevice;

    public Triangle(GraphicsDevice graphicsDevice, INode node1, INode node2, INode node3)
    {
        _graphicsDevice = graphicsDevice;

        _nodes = [node1, node2, node3];
    }

    public Triangle(GraphicsDevice graphicsDevice, INode[] nodes)
    {
        _graphicsDevice = graphicsDevice;
        _nodes = nodes;
    }

    public void Draw(BasicEffect effect)
    {
        var green = Color.Green;
        var vertices = new[]
        {
            new VertexPositionColor(_nodes[0].Position, green),
            new VertexPositionColor(_nodes[1].Position, green),

            new VertexPositionColor(_nodes[1].Position, green),
            new VertexPositionColor(_nodes[2].Position, green),

            new VertexPositionColor(_nodes[2].Position, green),
            new VertexPositionColor(_nodes[0].Position, green)
        };

        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 3);
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

    // ISaveable Implementation
    public JsonNode Serialize()
    {
        var jsonObject = new JsonObject
        {
            ["Nodes"] = new JsonArray(_nodes.Select(node => node.Serialize()).ToArray()),
            ["Colors"] = JsonUtils.ToJsonArray(_colors.Select(color => color.PackedValue))
        };
        return jsonObject;
    }

    public static Triangle Deserialize(GraphicsDevice graphicsDevice, JsonNode serializedData)
    //TODO remove graphicsDevice from constructor, use a setter and manager instead
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
        return new(graphicsDevice, deserializedNodes);

    }
}