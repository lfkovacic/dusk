
using System;
using System.Text.Json.Nodes;
using dusk.mejjiq.entities.@interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities;

public class GameEntity : IGameEntity
{

    private INode[] _nodes { get; set; }
    private IEdge[] _edges { get; set; }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect effect)
    {
        foreach (Edge edge in _edges) edge.Draw(graphicsDevice, effect);
    }

    public GameEntity(INode[] nodes, IEdge[] edges)
    {
        _nodes = nodes;
        _edges = edges;
    }

    public JsonNode Serialize()
    {
        throw new System.NotImplementedException();
    }

    public void Update(Node activeNode)
    {
        foreach (Edge e in _edges)
        {
            e.ApplyTension(activeNode);
        }
    }

    public void OnMouseDown(Vector2 mousePosition, ref Node activeNode)
    {

        foreach (Node node in _nodes)
        {
            node.OnMouseDown(mousePosition);
            if (node.IsDragging) activeNode = node;
        }
    }

    public void OnMouseUp(ref Node activeNode)
    {
        foreach (Node node in _nodes)
        {
            node.OnMouseUp();
            if (activeNode != null)
            {
                activeNode.IsDragging = false;
                activeNode = null;
            }
        }
    }
}
