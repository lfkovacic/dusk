using System;
using dusk.mejjiq.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk.mejjiq.ui.elements
{
    public class Button : IUIElement
    {
        public Vector2 Position {get; set;}
        public Vector2 Size {get; set;}

        public Vector2 DragOffset {get; set;}
        private Color _buttonColor = Color.Green;
        private Color _hoverColor = Color.DarkGreen;
        private Color _textColor = Color.Black;
        private bool _isHovering;
        private bool _wasPressed;

        private string _text;
        private SpriteFont _font;
        private Action _onClick;  // Store the Action

        // Store a reference to the EventManager to subscribe to mouse events
        private EventManager _eventManager;

        public Button(Vector2 position, Vector2 size, EventManager eventManager, Action onClick, string text = "", SpriteFont font = null)
        {
            Position = position;
            Size = size;
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
            if (new Rectangle(Position.ToPoint(), Size.ToPoint()).Contains(mousePosition) && !_wasPressed)
            {

            }
        }

        // Called when the mouse is released
        private void OnMouseReleased(Vector2 position)
        {
            var mousePosition = new Point((int)position.X, (int)position.Y);
            if (new Rectangle(Position.ToPoint(), Size.ToPoint()).Contains(mousePosition))
            {
                // Trigger the button click event
                _onClick?.Invoke();  // Invoke the Action
            }
        }

        private static Texture2D CreateDummyTexture(GraphicsDevice graphicsDevice)
        {
            var dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new[] { Color.White });
            return dummyTexture;
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            _isHovering = new Rectangle(Position.ToPoint(), Size.ToPoint()).Contains(mousePosition);
        }

        public void Draw(SpriteBatch spriteBatch, BasicEffect basicEffect)
        {
            var color = _isHovering ? _hoverColor : _buttonColor;

            // Draw the rectangle for the button
            spriteBatch.Draw(
                CreateDummyTexture(spriteBatch.GraphicsDevice),
                new Rectangle(Position.ToPoint(), Size.ToPoint()),
                color
            );

            // Draw the button text, if any
            if (!string.IsNullOrEmpty(_text) && _font != null)
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    Position.X + (Size.X - textSize.X) / 2,
                    Position.Y + (Size.Y - textSize.Y) / 2
                );
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
        }
    }
}