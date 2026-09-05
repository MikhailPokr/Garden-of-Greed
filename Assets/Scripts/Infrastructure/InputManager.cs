using UnityEngine;
using UnityEngine.InputSystem;

namespace Garden
{
    public class InputManager
    {
        private readonly InputAction _colorAction;
        private readonly InputAction _speedAction;
        private readonly InputAction _numAction;
        public bool Color { get; private set; }

        public InputManager()
        {
            _colorAction = InputSystem.actions.FindAction("Color");
            _colorAction.performed += OnColorAction;
            Color = false;
            
            _speedAction = InputSystem.actions.FindAction("Speed");
            _speedAction.performed += OnSpeedAction;
            
            _numAction = InputSystem.actions.FindAction("Num");
            _numAction.performed += OnNumAction; 
        }

        private void OnNumAction(InputAction.CallbackContext context)
        {
            string keyName = context.control.name;

            if (int.TryParse(keyName, out int number))
            {
                SignalBus<NumPressedSignal>.Fire(new NumPressedSignal(number));
            }
        }

        private void OnSpeedAction(InputAction.CallbackContext obj)
        {
            SignalBus<TimeSpeedChangedSignal>.Fire(new TimeSpeedChangedSignal(Mathf.FloorToInt(obj.ReadValue<float>())));
        }

        private void OnColorAction(InputAction.CallbackContext obj)
        {
            Color = !Color;
            SignalBus<ColorModeChangedSignal>.Fire(new(Color));
        }
    }
}