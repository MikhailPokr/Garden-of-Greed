using UnityEngine.InputSystem;

namespace Garden
{
    public class InputManager
    {
        private InputAction _colorAction;
        public bool _color;
        public InputManager()
        {
            _colorAction = InputSystem.actions.FindAction("Color");
            _colorAction.performed += OnColorAction;
            _color = false;
        }

        private void OnColorAction(InputAction.CallbackContext obj)
        {
            _color = !_color;
            SignalBus<ColorModeChangedSignal>.Fire(new(_color));
        }
    }
}