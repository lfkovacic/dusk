
using System;
using System.Text.Json.Nodes;
using dusk.mejjiq.entities.@interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities;

public class GameEntity(Triangle[] _segments) : IGameEntity
{

    private readonly Triangle[] _segments = _segments;

    public void Draw(BasicEffect effect)
    {
        foreach (Triangle triangle in _segments)
        {
            triangle.Draw(effect);
        }
    }

    public JsonNode Serialize()
    {
        throw new System.NotImplementedException();
    }

    public void Update(Node activeNode)
    {
        foreach (Triangle triangle in _segments)
        {
            foreach (Edge e in triangle.GetAllEdges())
            {
                e.ApplyTension(activeNode);
            }
        }
    }

    public void OnMouseDown(Vector2 mousePosition, ref Node activeNode)
    {

        foreach (Triangle triangle in _segments)
        {
            foreach (Node node in triangle._nodes)
            {
                node.OnMouseDown(mousePosition);
                if (node.IsDragging) activeNode = node;
            }
        }
    }

    public void OnMouseUp(ref Node activeNode)
    {
        foreach (Triangle triangle in _segments)
        {
            foreach (Node node in triangle._nodes)
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

    // public void OnMouseMove(Vector2 mousePosition, Node activeNode)
    // {
    //     foreach (Triangle triangle in _segments)
    //     {
    //         foreach (Node node in triangle._nodes)
    //         {
    //             activeNode.OnMouseMove(mousePosition);
    //         }
    //     }
    // }
}