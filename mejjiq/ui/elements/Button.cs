using System;
using dusk.mejjiq.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk.mejjiq.ui.elements
{
    public class Button
    {
        private Rectangle _bounds;
        private bool _isHovering;
        private bool _wasPressed;

        private string _text;
        private SpriteFont _font;
        private Action _onClick;  // Store the Action

        // Store a reference to the EventManager to subscribe to mouse events
        private EventManager _eventManager;

        public Button(Rectangle bounds, EventManager eventManager, Action onClick, string text = "", SpriteFont font = null)
        {
            _bounds = bounds;
            _eventManager = eventManager;
            _onClick = onClick;  // Pass the Action to the Button
            _text = text;
            _font = font;

            // Subscribe to mouse events in the EventManager
            _eventManager.MousePressed += OnMousePressed;
            _eventManager.MouseReleased += OnMouseReleased;
        }

        // Called when the mouse is pressed
        private void OnMousePressed(Vector2 position)
        {
            var mousePosition = new Point((int)position.X, (int)position.Y);
            if (_bounds.Contains(mousePosition) && !_wasPressed)
            {
               
            }
        }

        // Called when the mouse is released
        private void OnMouseReleased(Vector2 position)
        {
            var mousePosition = new Point((int)position.X, (int)position.Y);
            if (_bounds.Contains(mousePosition))
            {
                // Trigger the button click event
                _onClick?.Invoke();  // Invoke the Action
            }
        }

        public void Update(MouseState mouseState)
        {
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            _isHovering = _bounds.Contains(mousePosition);
        }

        public void Draw(SpriteBatch spriteBatch, Color buttonColor, Color hoverColor, Color textColor)
        {
            var color = _isHovering ? hoverColor : buttonColor;

            // Draw the rectangle for the button
            spriteBatch.Draw(
                CreateDummyTexture(spriteBatch.GraphicsDevice),
                _bounds,
                color
            );

            // Draw the button text, if any
            if (!string.IsNullOrEmpty(_text) && _font != null)
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    _bounds.X + (_bounds.Width - textSize.X) / 2,
                    _bounds.Y + (_bounds.Height - textSize.Y) / 2
                );
                spriteBatch.DrawString(_font, _text, textPosition, textColor);
            }
        }

        private static Texture2D CreateDummyTexture(GraphicsDevice graphicsDevice)
        {
            var dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new[] { Color.White });
            return dummyTexture;
        }
    }
}