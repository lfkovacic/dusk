using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace dusk.mejjiq.entities.@interface
{

    public interface IEdge
    {
        INode[] Nodes { get; set; }
        // public static abstract IEdge GetEdgeFromNodes(INode node0, INode node1);
        public Vector3 ToVector3(IEdge v);
        public JsonNode Serialize();

        // public static abstract IEdge Deserialize(JsonNode serializedData, INode[] nodes);
    }
}