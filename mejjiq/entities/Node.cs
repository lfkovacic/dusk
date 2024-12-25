using Microsoft.Xna.Framework;
using System.Collections.Generic;
using dusk.mejjiq.entities.@interface;
using dusk.mejjiq.interfaces;
using System.Text.Json.Nodes;
using dusk.mejjiq.util;
using System.IO;

namespace dusk.mejjiq.entities;

public class Node : INode, ISaveable
{
    public int ID { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    private readonly List<int> _connectedNodeIDs = new(); // Used for serialization
    private readonly List<INode> _connections = new();   // Runtime connections

    public Vector3 Position
    {
        get => new(X, Y, Z);
        set
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }
    }

    public Node(int id, Vector3 position)
    {
        ID = id;
        Position = position;
    }
    public Node(int id, float x, float y, float z)
    {
        ID = id;
        X = x;
        Y = y;
        Z = z;
    }

    public void AddConnection(INode node)
    {
        if (node == null || node == this || _connections.Contains(node))
            return;

        _connections.Add(node);
        if (node is Node n)
        {
            _connectedNodeIDs.Add(n.ID);
            n.AddConnection(this);
        }
    }

    public void RemoveConnection(INode node)
    {
        if (_connections.Remove(node))
        {
            if (node is Node n)
            {
                _connectedNodeIDs.Remove(n.ID);
                n.RemoveConnection(this);
            }
        }
    }

    public IReadOnlyList<INode> GetConnections() => _connections.AsReadOnly();

    // ISaveable Implementation
    public JsonNode Serialize()
    {
        var jsonObject = new JsonObject
        {
            [nameof(ID)] = ID,
            [nameof(X)] = X,
            [nameof(Y)] = Y,
            [nameof(Z)] = Z,
            ["ConnectedNodeIDs"] = JsonUtils.ToJsonArray(_connectedNodeIDs)
        };
        return jsonObject;
    }

    public static ISaveable Deserialize(JsonNode serializedData)
    {
        if (serializedData is not JsonObject obj)
            throw new InvalidDataException("Invalid node data");

        var node = new Node
        (
            obj[nameof(ID)]?.GetValue<int>() ?? 0, obj[nameof(X)]?.GetValue<float>() ?? 0,
            obj[nameof(Y)]?.GetValue<float>() ?? 0,
            obj[nameof(Z)]?.GetValue<float>() ?? 0
        );

        if (obj["ConnectedNodeIDs"] is JsonArray array)
        {
            node._connectedNodeIDs.AddRange(JsonUtils.FromJsonArray<int>(array));
        }

        return node;
    }

    public void RebuildConnections(Dictionary<int, Node> nodeLookup)
    {
        _connections.Clear();
        foreach (var id in _connectedNodeIDs)
        {
            if (nodeLookup.TryGetValue(id, out var connectedNode))
            {
                _connections.Add(connectedNode);
            }
        }
    }

}