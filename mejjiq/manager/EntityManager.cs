using System;
using System.Collections.Generic;
using dusk.mejjiq.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.manager
{
    public class EntityManager
    {
        private List<GameEntity> _entities;
        private GameEntity _activeEntity;

        private Node _activeNode;
        private bool _isAddingNewNode;
        private bool _isConnectingNodes;

        public EntityManager()
        {
            _entities = new List<GameEntity>();
            _isAddingNewNode = false;
            _isConnectingNodes = false;
        }

        public List<GameEntity> GetAllEntities(){
            return _entities;
        }

        // Properties for modes and active entity
        public bool IsAddingNewNode => _isAddingNewNode;
        public bool IsConnectingNodes => _isConnectingNodes;
        public GameEntity ActiveEntity => _activeEntity;

        // Set the active entity
        public void SetActiveEntity(GameEntity entity)
        {
            _activeEntity = entity;
        }

        // Start adding a new node
        public void StartAddingNewNode()
        {
            _isAddingNewNode = true;
            _isConnectingNodes = false; // Ensure we're not connecting nodes while adding
        }

        // Start connecting nodes
        public void StartConnectingNodes()
        {
            _isConnectingNodes = true;
            _isAddingNewNode = false; // Ensure we're not adding nodes while connecting
        }

        // Cancel current mode
        public void CancelAction()
        {
            _isAddingNewNode = false;
            _isConnectingNodes = false;
        }

        // Add a new node to the active entity
        public void AddNodeToActiveEntity(Vector3 position)
        {
            if (_activeEntity != null && _isAddingNewNode)
            {
                var newNode = new Node(_activeEntity.Nodes.Count, position);
                _activeEntity.AddNode(newNode);
            }
        }

        // Connect two nodes in the active entity
        public void ConnectNodesInActiveEntity(Node node1, Node node2)
        {
            if (_activeEntity != null && _isConnectingNodes)
            {
                var newEdge = new Edge(node1, node2, Vector3.Distance(node1.Position, node2.Position));
                _activeEntity.AddEdge(newEdge);
            }
        }

        // Update the entities (could be used for updates like dragging, etc.)
        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(_activeNode, gameTime);
            }
        }

        // Draw all entities
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(graphicsDevice, basicEffect);
            }
        }

        // Add a new entity to the manager
        public void AddEntity(GameEntity entity)
        {
            _entities.Add(entity);
        }
    }
}
