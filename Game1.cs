
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

    private GameEntity _gameEntity;

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

    private void LoadGameEntity()
    {
        // Create the first triangle (Triangle 1)
        var node1 = new Node(0, new Vector3(400, 400, 0));
        var node2 = new Node(1, new Vector3(500, 400, 0));
        var node3 = new Node(2, new Vector3(600, 200, 0));

        var triangle1 = new Triangle(GraphicsDevice, node1, node2, node3);

        // Create the second triangle (Triangle 2), sharing node2 and node3
        var node4 = new Node(3, new Vector3(700, 800, 0));  // New node for Triangle 2
        var triangle2 = new Triangle(GraphicsDevice, node2, node3, node4);

        // Create a GameEntity with these two triangles
        _gameEntity = new GameEntity([triangle1, triangle2]);
    }

    protected override void Initialize()
    {
        base.Initialize();
        // Load graphical settings from the config file
        int resolutionWidth = _configManager.GetIntValue("Graphics", "ResolutionWidth");
        int resolutionHeight = _configManager.GetIntValue("Graphics", "ResolutionHeight");
        bool fullscreen = _configManager.GetBoolValue("Graphics", "Fullscreen");

        // Apply resolution and fullscreen from config
        _graphics.PreferredBackBufferWidth = resolutionWidth;
        _graphics.PreferredBackBufferHeight = resolutionHeight;
        _graphics.IsFullScreen = fullscreen;

        _graphics.ApplyChanges();


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
        LoadGameEntity();
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
            _gameEntity.OnMouseDown(mousePosition, ref activeNode);
        }

        // Handle mouse up event (trigger only when the button is released)
        if (mouseState.LeftButton == ButtonState.Released && activeNode != null)
        {
            _gameEntity.OnMouseUp(ref activeNode);
        }

        // Handle mouse move event (if dragging)
        if (mouseState.LeftButton == ButtonState.Pressed && activeNode != null)
        {
            activeNode.OnMouseMove(mousePosition);
            Console.WriteLine(activeNode.Serialize());

        }
        _gameEntity.Update(activeNode);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _gameEntity.Draw(_basicEffect);
        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, Microsoft.Xna.Framework.ExitingEventArgs args)
    {
        base.OnExiting(sender, args);
    }
}
