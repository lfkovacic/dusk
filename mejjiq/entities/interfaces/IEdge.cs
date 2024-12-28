using dusk.mejjiq.entities.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.entities.interfaces
{
    public interface IEdge
    {
        /// <summary>
        /// Gets the first node connected by this edge.
        /// </summary>
        INode Node0 { get; }

        /// <summary>
        /// Gets the second node connected by this edge.
        /// </summary>
        INode Node1 { get; }

        float TensionCoefficient {get; set;}

        /// <summary>
        /// Apply tension to the edge's nodes
        /// </summary>
        void ApplyTension();


        /// <summary>
        /// Gets the length of the edge, calculated from its nodes.
        /// </summary>
        float Length { get; set; }

        /// <summary>
        /// Updates the edge's state (e.g., tension calculation).
        /// </summary>
        void Update();

        /// <summary>
        /// Draws the edge using the provided graphics context.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="basicEffect">The BasicEffect instance for rendering.</param>
        void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect);

        /// <summary>
        /// Serializes the edge's state to a JSON object.
        /// </summary>
        /// <returns>A JSON representation of the edge.</returns>
        string Serialize();
    }
}
