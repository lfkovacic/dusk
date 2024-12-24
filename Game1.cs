
using System.IO;
using dusk.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private BasicEffect _basicEffect;
    private Triangle _triangle;
    private VertexPositionColor[] _triangleVertices;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void SaveTriangleState()
    {
        var json = _triangle.ToJson();
        File.WriteAllText("triangle.json", json);
    }

    private void LoadTriangleState()
    {
        if (File.Exists("triangle.json"))
        {
            var json = File.ReadAllText("triangle.json");
            _triangle = Triangle.FromJson(GraphicsDevice, json);
        }
        else
        {
            // Create a default triangle if no saved data exists
            _triangle = new Triangle(
                GraphicsDevice,
                new Vector3(100, 200, 0), Color.Red,
                new Vector3(300, 200, 0), Color.Green,
                new Vector3(200, 100, 0), Color.Blue
            );
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Set up the BasicEffect
        _basicEffect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                                                            GraphicsDevice.Viewport.Height, 0, 0, 1),
            World = Matrix.Identity,
            View = Matrix.Identity
        };

        // Load the triangle state
        LoadTriangleState();
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            _triangle.UpdateVertex(2, new Vector3(mouseState.X, mouseState.Y, 0));
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _triangle.Draw(_basicEffect);

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, Microsoft.Xna.Framework.ExitingEventArgs args)
    {
        // Save the triangle state when the game closes
        SaveTriangleState();
        base.OnExiting(sender, args);
    }
}
