using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities.interfaces;


public interface IGameEntity{
    JsonNode Serialize();

    void Update(GameTime gameTime);
    void Draw(GraphicsDevice graphicsDevice, BasicEffect effect);
}