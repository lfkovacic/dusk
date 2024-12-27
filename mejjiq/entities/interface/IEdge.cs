using Microsoft.Xna.Framework;
using System.Text.Json.Nodes;

namespace dusk.mejjiq.entities.@interface;

public interface IEdge
{
    public static abstract IEdge GetEdgeFromNodes(INode node0, INode node1);


    public INode[] GetNodes();
    public Vector3 ToVector3(IEdge v);
    public JsonNode Serialize();

    public static abstract IEdge Deserialize(JsonNode serializedData, INode[] nodes);
}