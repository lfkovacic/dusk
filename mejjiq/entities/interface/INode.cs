using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace dusk.mejjiq.entities.@interface
{
    public interface INode
    {
        Vector3 Position { get; set; }
        void AddConnection(INode node);
        void RemoveConnection(INode node);

        JsonNode Serialize();
        IReadOnlyList<INode> GetConnections();
    }
}