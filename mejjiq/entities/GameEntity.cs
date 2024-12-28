
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using dusk.mejjiq.entities.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities;

public class GameEntity(List<INode> nodes, List<IEdge> edges) : IGameEntity
{

    public List<INode> Nodes { get; set; } = nodes;
    public List<IEdge> Edges { get; set; } = edges;

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect effect)
    {
        if (Edges.Count == 0)
            foreach (INode node in Nodes) node.Draw(graphicsDevice, effect);
        foreach (IEdge edge in Edges) edge.Draw(graphicsDevice, effect);
    }

    public JsonNode Serialize()
    {
        throw new System.NotImplementedException();
    }

    public void Update(GameTime gametime)
    {
        foreach (IEdge e in Edges)
        {
            e.ApplyTension();
        }
    }

    public void OnMouseDown(Vector2 mousePosition, ref INode activeNode)
    {

        foreach (INode node in Nodes)
        {
            node.OnMouseDown(mousePosition);
            if (node.IsDragging) activeNode = node;
        }
    }

    public void OnMouseUp(ref INode activeNode)
    {
        foreach (INode node in Nodes)
        {
            node.OnMouseUp();
            if (activeNode != null)
            {
                activeNode.IsDragging = false;
                activeNode = null;
            }
        }
    }

    public void AddNode(INode node)
    {
        Nodes.Add(node);
    }

    public void AddEdge(IEdge edge)
    {
        Edges.Add(edge);
    }

    public void AddEdge(INode node0, INode node1)
    {

        Edges.Add(new Edge(node0, node1, Vector3.Distance(node0.Position, node1.Position)));
    }

}
