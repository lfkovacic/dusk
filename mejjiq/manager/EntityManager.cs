using System;
using System.Collections.Generic;
using System.Linq;
using dusk.mejjiq.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk.mejjiq.manager
{
    public class EntityManager
    {

        //Any action perfomed with an entity is perfomed with the ACTIVE entity by default unless specified otherwise in method name
        //Same goes for nodes
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

        public List<GameEntity> GetAllEntities()
        {
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
            _activeEntity = null;
        }

        public Node GetActiveNode()
        {
            return _activeNode;
        }

        public void SetActiveNode(Node node)
        {
            _activeNode = node;

        }

        public Node GetNodeWithMouseInside(Vector2 mousePosition)
        {
            if (_activeEntity == null) return null;
            foreach (Node n in _activeEntity.Nodes)
            {
                if (n.IsMouseInside(mousePosition)) return n;
            }
            return null;
        }

        public Node GetNodeWithMouseInsideFromAllEntities(Vector2 mousePosition)
        {
            foreach (GameEntity entity in _entities)
            {
                foreach (Node n in entity.Nodes)
                {
                    bool isInside = n.IsMouseInside(mousePosition);
                    if (isInside)
                    {
                        if (_activeEntity != entity && _activeEntity != null) MergeEntities(entity);
                        return n;
                    }
                }
            }
            return null;
        }

        // Add a new node to the active entity
        public void AddNodeToActiveEntity(Vector3 position)
        {
            if (_activeEntity != null && _isAddingNewNode)
            {
                var newNode = new Node(_activeEntity.Nodes.Count, position);
                _activeEntity.AddNode(newNode);
                _activeNode = newNode;
            }
            if (_activeEntity != null && _isConnectingNodes)
            {
                var newNode = new Node(_activeEntity.Nodes.Count, position);
                _activeEntity.AddNode(newNode);
                _activeEntity.AddEdge(
                    new Edge(_activeNode, newNode, Vector3.Distance(_activeNode.Position, newNode.Position)));
                _activeNode = newNode;
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

        public void ConnectToActiveNode(Node node)
        {
            if (_activeNode != null)
            {
                var newEdge = new Edge(_activeNode, node, Vector3.Distance(_activeNode.Position, node.Position));
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



        public void MergeEntities(GameEntity entity)
        {
            GameEntity entityUnion = new GameEntity(
                _activeEntity.Nodes.Union(entity.Nodes).ToList(),
                _activeEntity.Edges.Union(entity.Edges).ToList()
            );
            _entities.Remove(_activeEntity);
            _entities.Remove(entity);

            _activeEntity = entityUnion;
            _entities.Add(_activeEntity);
        }
    }
}
