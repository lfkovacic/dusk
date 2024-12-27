using System.Text.Json.Nodes;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities.@interface;


public interface IGameEntity{
    JsonNode Serialize();

    void Update(Node activeNode);
    void Draw(GraphicsDevice graphicsDevice, BasicEffect effect);
}