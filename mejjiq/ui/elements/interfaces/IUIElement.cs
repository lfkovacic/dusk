using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dusk.mejjiq.ui.elements
{
    /// <summary>
    /// Defines the basic interface for a UI element that can be part of a container like a Panel.
    /// </summary>
    public interface IUIElement
    {
        /// <summary>
        /// Updates the UI element's state (e.g., for interactions like clicks).
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Update(GameTime gameTime);

        public Vector2 Position {get; set;}
        public Vector2 Size {get; set;}
        public Vector2 DragOffset {get; set;}

        /// <summary>
        /// Draws the UI element to the screen.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing 2D content.</param>
        /// <param name="basicEffect">The BasicEffect used for drawing 3D components.</param>
        void Draw(SpriteBatch spriteBatch, BasicEffect basicEffect);
    }
}
