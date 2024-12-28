using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace dusk.mejjiq.ui.elements
{
    /// <summary>
    /// A UI Panel that contains its own relative coordinates, can hold child elements,
    /// and can be moved around the screen.
    /// </summary>
    public class Panel : IPanel
    {
        // Panel properties
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        private List<IUIElement> _childElements;
        private bool _isDragging;
        private Vector2 _dragOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="position">The initial position of the panel.</param>
        /// <param name="size">The size of the panel.</param>
        public Panel(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
            _childElements = new List<IUIElement>();
            _isDragging = false;
        }

        /// <summary>
        /// Adds a UI element to the panel (e.g., buttons, labels).
        /// The position of child elements is relative to the panel.
        /// </summary>
        /// <param name="element">The UI element to add.</param>
        public void AddChild(IUIElement element)
        {
            element.Position+=Position;
            _childElements.Add(element);
        }

        /// <summary>
        /// Handles mouse input to allow dragging the panel.
        /// </summary>
        /// <param name="mouseState">The current mouse state.</param>
        public void HandleInput(MouseState mouseState)
        {
            var mousePosition = mouseState.Position.ToVector2();
            if (_isDragging)
            {
                // Adjust panel's position based on mouse movement
                Position = mousePosition-_dragOffset;
                foreach (IUIElement child in _childElements)
                {
                    child.Position = mousePosition-child.DragOffset;
                }
            }
            else
            {
                // Check if the mouse is over the panel and the left mouse button is pressed
                if (new Rectangle(Position.ToPoint(), Size.ToPoint()).Contains(mouseState.X, mouseState.Y)
                    && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _isDragging = true;
                    _dragOffset = mousePosition-Position;
                    foreach (IUIElement child in _childElements){
                        child.DragOffset = mousePosition-child.Position;
                    }

                }
            }

            // If the mouse button is released, stop dragging
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isDragging = false;
            }
        }

        /// <summary>
        /// Updates the panel and its child elements (e.g., handling button clicks).
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(GameTime gameTime)
        {
            // Update the panel's child elements
            foreach (var child in _childElements)
            {
                child.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the panel, its contents, and other elements like buttons.
        /// Child elements are drawn relative to the panel's position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing 2D content.</param>
        /// <param name="basicEffect">The BasicEffect used for drawing 3D components.</param>
        public void Draw(SpriteBatch spriteBatch, BasicEffect basicEffect)
        {
            // Draw a simple background rectangle for the panel
            spriteBatch.Draw(CreateDummyTexture(spriteBatch.GraphicsDevice), new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.Gray);

            // Draw all child elements (e.g., buttons), positioning them relative to the panel
            foreach (var child in _childElements)
            {
                // Here, we offset the child element's position by the panel's position to make it relative
                child.Draw(spriteBatch, basicEffect);  // Pass the panel's position to adjust the drawing position
            }
        }

        /// <summary>
        /// Creates a dummy texture for panel background drawing.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device used to create the texture.</param>
        /// <returns>A 1x1 dummy texture.</returns>
        private static Texture2D CreateDummyTexture(GraphicsDevice graphicsDevice)
        {
            var texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });
            return texture;
        }
    }
}
