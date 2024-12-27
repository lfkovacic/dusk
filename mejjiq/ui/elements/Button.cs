using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dusk.mejjiq.ui.elements
{
    public class Button
    {
        private Rectangle _bounds;
        private Action _onClick;
        private bool _isHovering;
        private bool _wasPressed;

        private string _text;
        private SpriteFont _font;

        public Button(Rectangle bounds, Action onClick, string text = "", SpriteFont font = null)
        {
            _bounds = bounds;
            _onClick = onClick;
            _text = text;
            _font = font;
        }

        public void Update(MouseState mouseState)
        {
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            _isHovering = _bounds.Contains(mousePosition);

            if (_isHovering && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!_wasPressed)
                {
                    _onClick?.Invoke();
                }
                _wasPressed = true;
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                _wasPressed = false;
            }
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