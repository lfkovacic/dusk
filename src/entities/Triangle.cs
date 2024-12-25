using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.src.entities;
public class Triangle
{
    public class VertexData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Color Color { get; set; }

        [JsonIgnore] // Ignore in JSON; only for internal use
        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
    }

    private VertexPositionColor[] _vertices;

    private GraphicsDevice _graphicsDevice;

    public Triangle(GraphicsDevice graphicsDevice, Vector3 point1, Color color1,
                    Vector3 point2, Color color2, Vector3 point3, Color color3)
    {
        _graphicsDevice = graphicsDevice;

        // Define vertices for the triangle outline (two vertices per edge)
        _vertices = new VertexPositionColor[6];
        _vertices[0] = new VertexPositionColor(point1, color1);
        _vertices[1] = new VertexPositionColor(point2, color1);

        _vertices[2] = new VertexPositionColor(point2, color2);
        _vertices[3] = new VertexPositionColor(point3, color2);

        _vertices[4] = new VertexPositionColor(point3, color3);
        _vertices[5] = new VertexPositionColor(point1, color3);
    }

    public void UpdateVertex(int vertexIndex, Vector3 newPosition)
    {
        if (vertexIndex < 0 || vertexIndex >= 3) return;

        if (vertexIndex == 0)
        {
            _vertices[0].Position = newPosition;
            _vertices[5].Position = newPosition;
        }
        else if (vertexIndex == 1)
        {
            _vertices[1].Position = newPosition;
            _vertices[2].Position = newPosition;
        }
        else if (vertexIndex == 2)
        {
            _vertices[3].Position = newPosition;
            _vertices[4].Position = newPosition;
        }
    }

    public void Draw(BasicEffect effect)
    {
        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _vertices, 0, 3);
        }
    }

    // Serialize the triangle to JSON
    public string ToJson()
    {
        var vertexData = new[]
        {
            new VertexData { Position = _vertices[0].Position, Color = _vertices[0].Color },
            new VertexData { Position = _vertices[2].Position, Color = _vertices[2].Color },
            new VertexData { Position = _vertices[4].Position, Color = _vertices[4].Color }
        };

        return JsonSerializer.Serialize(vertexData, new JsonSerializerOptions { WriteIndented = true });
    }

    // Load triangle from JSON
    public static Triangle FromJson(GraphicsDevice graphicsDevice, string json)
    {
        var vertexData = JsonSerializer.Deserialize<VertexData[]>(json);
        if (vertexData == null || vertexData.Length != 3)
            throw new InvalidDataException("Invalid triangle data");

        return new Triangle(
            graphicsDevice,
            new Vector3(vertexData[0].X, vertexData[0].Y, vertexData[0].Z), vertexData[0].Color,
            new Vector3(vertexData[1].X, vertexData[1].Y, vertexData[1].Z), vertexData[1].Color,
            new Vector3(vertexData[2].X, vertexData[2].Y, vertexData[2].Z), vertexData[2].Color
        );
    }
}
