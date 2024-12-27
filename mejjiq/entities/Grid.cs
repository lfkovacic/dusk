using System.Linq;
using dusk.mejjiq.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities;

public class Grid
{
    private int _width = ConfigManager.GetIntValue("Graphics", "ResolutionWidth");
    private int _height = ConfigManager.GetIntValue("Graphics", "ResolutionHeight");
    private int _gridResolution = 20;

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect effect)
    {
        var green = Color.DarkGreen;
        for (int i = 0; i < _width; i+=_gridResolution)
        {
            var posX0 = new VertexPositionColor(new Vector3(0, i, 0), green);
            var posX1 = new VertexPositionColor(new Vector3(_width, i, 0), green);
            var posY0 = new VertexPositionColor(new Vector3(i,0, 0), green);
            var posY1 = new VertexPositionColor(new Vector3(i,_height, 0), green);
            var verticesX = new[]
            {
                posX0,
                posX1
            }.ToArray();
            var verticesY = new[]{
                posY0,
                posY1
            }.ToArray();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, verticesX, 0, 1);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, verticesY, 0, 1);
            }
        }
    }
}