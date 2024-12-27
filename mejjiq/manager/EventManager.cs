using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace dusk.mejjiq.manager
{
    public class EventManager
    {
        // Define events for mouse and keyboard actions
        public event Action<Vector2> MouseMoved;
        public event Action<Vector2> MousePressed;
        public event Action<Vector2> MouseReleased;
        public event Action<Keys> KeyPressed;
        public event Action<Keys> KeyReleased;

        private MouseState _previousMouseState;
        private KeyboardState _previousKeyboardState;

        public EventManager()
        {
            _previousMouseState = Mouse.GetState();
            _previousKeyboardState = Keyboard.GetState();
        }

        // Update method, should be called in Game's Update method
        public void Update(GameTime gameTime)
        {
            // Handle mouse events
            HandleMouseEvents();

            // Handle keyboard events
            HandleKeyboardEvents();
        }

        private void HandleMouseEvents()
        {
            var currentMouseState = Mouse.GetState();

            // Mouse movement
            if (currentMouseState.Position != _previousMouseState.Position)
            {
                MouseMoved?.Invoke(new Vector2(currentMouseState.X, currentMouseState.Y));
            }

            // Mouse pressed
            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                MousePressed?.Invoke(new Vector2(currentMouseState.X, currentMouseState.Y));
            }

            // Mouse released
            if (currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                MouseReleased?.Invoke(new Vector2(currentMouseState.X, currentMouseState.Y));
            }

            // Update the previous mouse state
            _previousMouseState = currentMouseState;
        }

        private void HandleKeyboardEvents()
        {
            var currentKeyboardState = Keyboard.GetState();

            // Check for key presses
            foreach (var key in Enum.GetValues(typeof(Keys)))
            {
                if (currentKeyboardState.IsKeyDown((Keys)key) && _previousKeyboardState.IsKeyUp((Keys)key))
                {
                    KeyPressed?.Invoke((Keys)key);
                }

                // Check for key releases
                if (currentKeyboardState.IsKeyUp((Keys)key) && _previousKeyboardState.IsKeyDown((Keys)key))
                {
                    KeyReleased?.Invoke((Keys)key);
                }
            }

            // Update the previous keyboard state
            _previousKeyboardState = currentKeyboardState;
        }
    }
}