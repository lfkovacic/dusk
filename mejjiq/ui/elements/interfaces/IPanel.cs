using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.ui.elements
{
    /// <summary>
    /// Defines the basic interface for a UI Panel element.
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// Gets or sets the position of the panel.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the panel.
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// Draws the panel and its contents (such as UI elements like buttons).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for 2D drawing.</param>
        /// <param name="basicEffect">The BasicEffect used for drawing 3D elements or UI components that require it.</param>
        void Draw(SpriteBatch spriteBatch, BasicEffect basicEffect);

        /// <summary>
        /// Updates the panel's state, including any child elements' interactions (e.g., buttons).
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Update(GameTime gameTime);
    }
}
