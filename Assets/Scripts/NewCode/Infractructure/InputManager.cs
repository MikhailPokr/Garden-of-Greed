using UnityEngine;
using UnityEngine.InputSystem;

namespace Garden
{
    public class InputManager
    {
        private InputAction _colorAction;
        private InputAction _speedAction;
        public bool Color { get; private set; }

        public InputManager()
        {
            _colorAction = InputSystem.actions.FindAction("Color");
            _colorAction.performed += OnColorAction;
            Color = false;
            
            _speedAction = InputSystem.actions.FindAction("Speed");
            _speedAction.performed += OnSpeedAction;
        }

        private void OnSpeedAction(InputAction.CallbackContext obj)
        {
            Time.timeScale += obj.ReadValue<float>() * 1f;
            Debug.Log(Time.timeScale);
        }

        private void OnColorAction(InputAction.CallbackContext obj)
        {
            Color = !Color;
            SignalBus<ColorModeChangedSignal>.Fire(new(Color));
        }
    }
}