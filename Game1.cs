using System;
using dusk.mejjiq.entities;
using dusk.mejjiq.manager;
using dusk.mejjiq.ui.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk;

/// <summary>
/// The main game class that handles initialization, updating, and drawing game components.
/// </summary>
public class Game1 : Game
{
    private GraphicsDeviceManager _graphicsManager;
    private EventManager _eventManager;
    private EntityManager _entityManager;
    private BasicEffect _basicEffect;
    private Grid _grid;

    private SpriteFont _defaultFont;
    private SpriteBatch _spriteBatch;

    private Panel _panel;
    private Button _button1; // TODO: Move buttons into a dedicated ButtonManager
    private Button _button2; // TODO: Move buttons into a dedicated ButtonManager

    /// <summary>
    /// Initializes a new instance of the <see cref="Game1"/> class.
    /// </summary>
    public Game1()
    {
        _graphicsManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Loads game content such as fonts, textures, and other resources.
    /// </summary>
    protected override void LoadContent()
    {
        _defaultFont = Content.Load<SpriteFont>("Arial");
    }

    /// <summary>
    /// Initializes the grid used in the game.
    /// </summary>
    private void LoadGrid()
    {
        _grid = new Grid();
    }

    /// <summary>
    /// Loads the initial set of game entities and adds them to the <see cref="EntityManager"/>.
    /// </summary>
    private void LoadGameEntities()
    {
        // Hardcoded definition of an entity for testing purposes
        //TODO: finish json (de)serialization
        var node0 = new Node(0, new Vector3(400, 400, 0));
        var node1 = new Node(1, new Vector3(500, 400, 0));
        var node2 = new Node(2, new Vector3(600, 200, 0));
        var node3 = new Node(3, new Vector3(700, 800, 0));

        var edge0 = new Edge(node0, node1, 100f);
        var edge1 = new Edge(node1, node2, 100f);
        var edge2 = new Edge(node2, node0, 100f);
        var edge3 = new Edge(node1, node3, 50f);
        var edge4 = new Edge(node3, node2, 50f);

        var gameEntity = new GameEntity(
            [node0, node1, node2, node3],
            [edge0, edge1, edge2, edge3, edge4]
        );

        // Add to the list of entities
        _entityManager = new EntityManager(_eventManager);
        _entityManager.AddEntity(gameEntity);
    }

    /// <summary>
    /// Handles global mouse press events.
    /// </summary>
    /// <param name="position">The position of the mouse at the time of the press.</param>
    private void OnMousePressed(Vector2 position)
    {
        // Global mouse press events
    }

    /// <summary>
    /// Handles global mouse movement events.
    /// </summary>
    /// <param name="position">The current position of the mouse.</param>
    private void OnMouseMoved(Vector2 position)
    {
        // Global mouse move events
    }

    /// <summary>
    /// Handles global mouse release events.
    /// </summary>
    /// <param name="position">The position of the mouse at the time of the release.</param>
    private void OnMouseReleased(Vector2 position)
    {
        // Global mouse release events
    }

    /// <summary>
    /// Handles global key press events.
    /// </summary>
    /// <param name="k">The key that was pressed.</param>
    private void OnKeyPressed(Keys k)
    {
        // Global key press events
    }

    /// <summary>
    /// Handles global key release events.
    /// </summary>
    /// <param name="k">The key that was released.</param>
    private void OnKeyReleased(Keys k)
    {
        // Global key release events
    }

    /// <summary>
    /// Initializes game components, config settings, and managers.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        // Load graphical settings from the config file
        ConfigManager.LoadConfig();

        _eventManager = new EventManager();
        // Subscribe to the mouse events
        _eventManager.MouseMoved += OnMouseMoved;
        _eventManager.MousePressed += OnMousePressed;
        _eventManager.MouseReleased += OnMouseReleased;

        // Subscribe to the keyboard events
        _eventManager.KeyPressed += OnKeyPressed;
        _eventManager.KeyReleased += OnKeyReleased;

        LoadContent();
        _panel = new Panel(new Vector2(240, 20), new Vector2(400, 400));
    
        _panel.AddChild(new Button(
            new Vector2(20, 20),new Vector2( 200, 60),
            _eventManager,
            new Action(() => Console.WriteLine("Button pressed!")),
            "test",
            _defaultFont
        ));
        _panel.AddChild(_button2 = new Button(
            new Vector2(20, 100), new Vector2(200, 60),
            _eventManager,
            new Action(() => { _entityManager.StartAddingNewNode(); _entityManager.SetActiveEntity(null); }),
            "Add new node",
            _defaultFont
        ));

        int resolutionWidth = ConfigManager.GetIntValue("Graphics", "ResolutionWidth");
        int resolutionHeight = ConfigManager.GetIntValue("Graphics", "ResolutionHeight");
        bool fullscreen = ConfigManager.GetBoolValue("Graphics", "Fullscreen");

        // Apply resolution and fullscreen from config
        _graphicsManager.PreferredBackBufferWidth = resolutionWidth;
        _graphicsManager.PreferredBackBufferHeight = resolutionHeight;
        _graphicsManager.IsFullScreen = fullscreen;

        _graphicsManager.ApplyChanges();
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Set up the BasicEffect
        _basicEffect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                                                            GraphicsDevice.Viewport.Height, 0, 0, 1),
            World = Matrix.Identity,
            View = Matrix.Identity
        };

        LoadGrid();
        LoadGameEntities();
    }

    /// <summary>
    /// Updates game logic such as entities, events, and UI components.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _eventManager.Update(gameTime);

        _entityManager.Update(gameTime);
        var mouseState = Mouse.GetState();
        _panel.HandleInput(mouseState);
        _panel.Update(gameTime);
    }

    /// <summary>
    /// Draws all game components to the screen.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _grid.Draw(GraphicsDevice, _basicEffect);

        _entityManager.Draw(GraphicsDevice, _basicEffect);

        _spriteBatch.Begin();
        _panel.Draw(_spriteBatch, null);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    /// <summary>
    /// Handles game exit logic.
    /// </summary>
    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        base.OnExiting(sender, args);
    }
}
