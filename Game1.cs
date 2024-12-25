
using System.IO;
using dusk.mejjiq.entities;
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


    private void LoadTriangleState()
    {
        
            // Create a default triangle if no saved data exists
            _triangle = new Triangle(
                GraphicsDevice,
                new Node(0, new Vector3(100, 200, 0)),
                new Node(1, new Vector3(300, 200, 0)),
                new Node(2, new Vector3(200, 100, 0))
            );
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

        // if (mouseState.LeftButton == ButtonState.Pressed)
        // {
        //     _triangle.UpdateVertex(2, new Vector3(mouseState.X, mouseState.Y, 0));
        // }

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
        base.OnExiting(sender, args);
    }
}
