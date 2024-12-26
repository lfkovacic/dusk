
using System;
using System.IO;
using dusk.mejjiq.entities;
using dusk.mejjiq.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private ConfigManager _configManager;
    private BasicEffect _basicEffect;
    private Triangle _triangle;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _configManager = new ConfigManager("./config.ini");
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }


    private void LoadTriangleState()
    {

        _triangle = new Triangle(
            GraphicsDevice,
            new Node(0, new Vector3(100, 200, 0)),
            new Node(1, new Vector3(300, 200, 0)),
            new Node(2, new Vector3(200, 100, 0))
        );
        _triangle.GetEdge(0).MinLength = 50f;
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
    Node activeNode = null;

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);



        // Get the current mouse state and position
        var mouseState = Mouse.GetState();
        var mousePosition = new Vector2(mouseState.X, mouseState.Y);

        // Debug print the mouse state for each update
        Console.WriteLine($"Current Mouse State: {mouseState.LeftButton}");

        // Handle mouse down event (detect when the mouse is pressed down)
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            foreach (Node node in _triangle._nodes)
            {
                node.OnMouseDown(mousePosition);
                if (node.IsDragging) activeNode = node;
            }
        }

        // Handle mouse up event (trigger only when the button is released)
        if (mouseState.LeftButton == ButtonState.Released && activeNode != null)
        {
            // Console.WriteLine("Node is supposed to do the OnMouseUp thingy: " + activeNode.Serialize());
            activeNode.OnMouseUp();  // Trigger OnMouseUp when the mouse is released
            activeNode.IsDragging = false;
            activeNode = null;
        }

        // Handle mouse move event (if dragging)
        if (mouseState.LeftButton == ButtonState.Pressed && activeNode != null)
        {
            activeNode.OnMouseMove(mousePosition);

        }
        foreach (Edge edge in _triangle.GetAllEdges())
        {
            edge.ApplyTension(activeNode);
        }

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _triangle.Draw(_basicEffect);

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, Microsoft.Xna.Framework.ExitingEventArgs args)
    {
        base.OnExiting(sender, args);
    }
}
