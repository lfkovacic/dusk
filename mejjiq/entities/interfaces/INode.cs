using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities.interfaces
{
    public interface INode
    {
        /// <summary>
        /// Gets or sets the unique identifier of the node.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets or sets the position of the node.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Indicates whether the node is currently being dragged.
        /// </summary>
        bool IsDragging { get; set; }

        /// <summary>
        /// Called when the mouse interacts with the node.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse.</param>
        void OnMouseDown(Vector2 mousePosition);

        /// <summary>
        /// Called when the mouse releases the node.
        /// </summary>
        void OnMouseUp();

        /// <summary>
        /// Called when the mouse moves while interacting with the node.
        /// </summary>
        /// <param name="mousePosition">The new mouse position.</param>
        void OnMouseMove(Vector2 mousePosition);

        /// <summary>
        /// Serialize the node's state to a JSON object.
        /// </summary>
        /// <returns>A JSON representation of the node.</returns>
        string Serialize();

        /// <summary>
        /// Applies force or updates the node in response to tension.
        /// </summary>
        /// <param name="activeNode">The currently active node for context.</param>
        void ApplyTension(INode activeNode);

        /// <summary>
        /// Draws the node, ikr?
        /// <summary>
        void Draw(GraphicsDevice graphicsDevide, BasicEffect basicEffect);
    }
}