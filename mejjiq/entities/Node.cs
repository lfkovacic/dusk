using Microsoft.Xna.Framework;
using System.Collections.Generic;
using dusk.mejjiq.entities.@interface;
using dusk.mejjiq.interfaces;
using System.Text.Json.Nodes;
using dusk.mejjiq.util;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using dusk.mejjiq.math;
using System;
using dusk.mejjiq.manager;

namespace dusk.mejjiq.entities;

public class Node : INode, ISaveable
{
    public int ID { get; set; }

    //Position
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    //Velocity
    public float Dx { get; private set; }
    public float Dy { get; private set; }
    public float Dz { get; private set; }

    public int NodeRadius { get; set; } = 10;
    public bool IsDragging { get; set; } = false;  // To track if the node is being dragged
    private readonly List<int> _connectedNodeIDs = []; // Used for serialization
    private readonly List<INode> _connections = [];   // Runtime connections

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

    public Vector3 CurrentVelocity
    {
        get => new(Dx, Dy, Dz);
        set
        {
            Dx = value.X;
            Dy = value.Y;
            Dz = value.Z;
        }
    }

    private readonly float _vectorUpdateThreshold = ConfigManager.GetFloatValue("Physics", "VectorUpdateThreshold");



    public Node() { }

    public Node(int id, Vector3 position)
    {
        ID = id;
        Position = position;
        CurrentVelocity = new Vector3(0, 0, 0);

    }
    // Method to check if the mouse is inside the node's circle
    public bool IsMouseInside(Vector2 mousePosition)
    {
        // Calculate the distance between the mouse position and the node's center
        float distance = Vector2.Distance(new Vector2(X, Y), mousePosition);
        return distance <= NodeRadius;
    }

    // Method to handle mouse down events (start dragging)
    public void OnMouseDown(Vector2 mousePosition)
    {

        if (IsMouseInside(mousePosition) && !IsDragging)
        {
            IsDragging = true;
        }
    }

    // Method to handle mouse up events (stop dragging)
    public void OnMouseUp()
    {
        IsDragging = false;
    }

    // Method to handle mouse move events (move the node if dragging)
    public void OnMouseMove(Vector2 mousePosition)
    {
        if (IsDragging)
        {
            UpdateWithVector(MathUtils.GetTensionVector(Position, new Vector3(mousePosition, 0), 20, 50f));
            ApplyFriction(0.8f);
        }
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

    public void UpdateWithVector(Vector3 vector)
    {
        CurrentVelocity += vector;
        Update();
    }

    public void Update()
    {
        if (CurrentVelocity == Vector3.Zero) return; 
        if (CurrentVelocity.Length() < _vectorUpdateThreshold) CurrentVelocity = Vector3.Zero; //Don't bother calculating velocity below a certain theshold
        Position += CurrentVelocity;
    }

    public void ApplyFriction(float friction)
    {
        CurrentVelocity *= friction;
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect effect)
    {
        var radius = NodeRadius;
        var CircleResolution = 50;
        var color = new Color(0x00ff00);

        // Generate the circle's vertices (edges)
        List<Vector3> circleEdges = MathUtils.GenerateCircleEdges(new Vector3(X, Y, Z), radius, CircleResolution);

        // Convert List<Vector3> to VertexPositionColor array
        VertexPositionColor[] vertices = new VertexPositionColor[circleEdges.Count * 2]; // Each edge is a line

        for (int i = 0; i < circleEdges.Count; i++)
        {
            // Connect the current point to the next point (looping back to 0 after the last point)
            int nextIndex = (i + 1) % circleEdges.Count;

            // Create the line between two points (current and next point)
            vertices[i * 2] = new VertexPositionColor(circleEdges[i], color);
            vertices[i * 2 + 1] = new VertexPositionColor(circleEdges[nextIndex], color);
        }

        // Draw the circle using BasicEffect
        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, circleEdges.Count);
        }
    }
    public static ISaveable Deserialize(JsonNode serializedData)
    {
        if (serializedData is not JsonObject obj)
            throw new InvalidDataException("Invalid node data");

        var node = new Node
        (
            obj[nameof(ID)]?.GetValue<int>() ?? 0,
            new Vector3
            (
                obj[nameof(X)]?.GetValue<float>() ?? 0,
                obj[nameof(Y)]?.GetValue<float>() ?? 0,
                obj[nameof(Z)]?.GetValue<float>() ?? 0
            )
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

    public void MoveByVector(Vector3 vector)
    {
        Position += vector;
    }

}