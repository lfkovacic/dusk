
using System;
using System.Collections.Generic;
using System.Linq;
using dusk.mejjiq.entities;
using dusk.mejjiq.manager;
using dusk.mejjiq.ui.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphicsManager;
    private EventManager _eventManager;
    private EntityManager _entityManager;
    private BasicEffect _basicEffect;
    private Grid _grid;
    private Node _activeNode = null;

    private SpriteFont _defaultFont;
    private SpriteBatch _spriteBatch;

    private Button _button1;
    private Button _button2;

    public Game1()
    {
        _graphicsManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _defaultFont = Content.Load<SpriteFont>("Arial");
    }

    private void LoadGrid()
    {
        _grid = new Grid();
    }

    private void LoadGameEntities()
    {
        // Create the first triangle (Triangle 1)
        var node0 = new Node(0, new Vector3(400, 400, 0));
        var node1 = new Node(1, new Vector3(500, 400, 0));
        var node2 = new Node(2, new Vector3(600, 200, 0));
        var node3 = new Node(3, new Vector3(700, 800, 0));

        var edge0 = new Edge(node0, node1, 100f);
        var edge1 = new Edge(node1, node2, 100f);
        var edge2 = new Edge(node2, node0, 100f);
        var edge3 = new Edge(node1, node3, 50f);
        var edge4 = new Edge(node3, node2, 50f);

        // Create a GameEntity with these triangles
        var gameEntity = new GameEntity(
            [node0, node1, node2, node3],
            [edge0, edge1, edge2, edge3, edge4]
        );

        // Add to the list of entities
        _entityManager = new EntityManager();
        _entityManager.AddEntity(gameEntity);
    }

    private void OnMousePressed(Vector2 position)
    {
        if (_entityManager.IsAddingNewNode)
        {
            if (_entityManager.ActiveEntity == null)
            {
                var entity = new GameEntity([], []);
                _entityManager.SetActiveEntity(entity);
                _entityManager.AddNodeToActiveEntity(new Vector3(position, 0));
                _entityManager.AddEntity(entity);
                _entityManager.StartConnectingNodes();
            } else {
                _entityManager.StartConnectingNodes();
            }
        }
        if (_entityManager.IsConnectingNodes && !_entityManager.GetActiveNode().IsMouseInside(position))
        {

            _entityManager.AddNodeToActiveEntity(new Vector3(position, 0));
            _entityManager.StartAddingNewNode();
        }
        //TODO: Connecting nodes
        foreach (var entity in _entityManager.GetAllEntities())
        {
            entity.OnMouseDown(position, ref _activeNode);
        }

    }

    private void OnMouseMoved(Vector2 position)
    {
        if (_activeNode == null) return;
        _activeNode.OnMouseMove(position);
    }

    private void OnMouseReleased(Vector2 position)
    {
        foreach (var entity in _entityManager.GetAllEntities())
        {
            entity.OnMouseUp(ref _activeNode);
        }
    }

    private void OnKeyPressed(Keys k)
    {

    }
    private void OnKeyReleased(Keys k)
    {

    }

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

        _button1 = new Button(
            new Rectangle(20, 20, 200, 60),
            _eventManager,
            new Action(() => Console.WriteLine("Button pressed!")),
            "test",
            _defaultFont
            );

        _button2 = new Button(
            new Rectangle(20, 100, 200, 60),
            _eventManager,
            new Action(() => _entityManager.StartAddingNewNode()),
            "Add new node",
            _defaultFont
        );

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



        // Load the triangle state
        LoadGameEntities();
    }


    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _eventManager.Update(gameTime);

        _button1.Update(Mouse.GetState());
        _button2.Update(Mouse.GetState());

        foreach (GameEntity entity in _entityManager.GetAllEntities())
        {
            entity.Update(_activeNode, gameTime);
        }

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);


        _grid.Draw(GraphicsDevice, _basicEffect);

        _entityManager.Draw(GraphicsDevice, _basicEffect);
        _spriteBatch.Begin();
        _button1.Draw(_spriteBatch, Color.Green, Color.DarkGreen, Color.Black);
        _button2.Draw(_spriteBatch, Color.Green, Color.DarkGreen, Color.Black);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        base.OnExiting(sender, args);
    }
}
