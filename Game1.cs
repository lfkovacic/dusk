using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private BasicEffect _basicEffect;
    private VertexPositionColor[] _triangleVertices;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Define the vertices for the triangle outline (two vertices per edge)
        _triangleVertices = new VertexPositionColor[6];
        _triangleVertices[0] = new VertexPositionColor(new Vector3(100, 200, 0), Color.Red); // Bottom-left
        _triangleVertices[1] = new VertexPositionColor(new Vector3(300, 200, 0), Color.Red); // Bottom-right

        _triangleVertices[2] = new VertexPositionColor(new Vector3(300, 200, 0), Color.Green); // Bottom-right
        _triangleVertices[3] = new VertexPositionColor(new Vector3(200, 100, 0), Color.Green); // Top

        _triangleVertices[4] = new VertexPositionColor(new Vector3(200, 100, 0), Color.Blue); // Top
        _triangleVertices[5] = new VertexPositionColor(new Vector3(100, 200, 0), Color.Blue); // Bottom-left

        // Set up the BasicEffect
        _basicEffect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                                                            GraphicsDevice.Viewport.Height, 0, 0, 1),
            World = Matrix.Identity,
            View = Matrix.Identity
        };
    }

    protected override void Update(GameTime gameTime)
    {
        // Get the current mouse state
        var mouseState = Mouse.GetState();

        // Check if the left mouse button is pressed
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            // Update the position of the "Top" vertex (index 3 and 4 in the array)
            Vector3 mousePosition = new Vector3(mouseState.X, mouseState.Y, 0);
            _triangleVertices[3].Position = mousePosition; // Update the top vertex
            _triangleVertices[4].Position = mousePosition; // Also update the connected edge
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach (var pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _triangleVertices, 0, 3); // Three lines
        }

        base.Draw(gameTime);
    }

}
