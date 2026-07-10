using System;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class SpeedButtonsController : MonoBehaviour
    {
        [SerializeField] private UISpeedButton[]  _speedButtons;
        
        private SpeedController _speedController;
        
        public void Init(SpeedController speedController)
        {
            _speedController = speedController;
            if (_speedController.Speeds.Length != _speedButtons.Length)
                throw new Exception("the speed buttons do not match the speeds");
            for (var i = 0; i < _speedButtons.Length; i++)
            {
                var speedButton = _speedButtons[i];
                speedButton.Init();
                speedButton.OnClick += OnClick;
            }

            _speedController.SpeedChange += SpeedSpeedChange;
            OnClick(1);
        }

        private void SpeedSpeedChange()
        {
            foreach (var speedButton in _speedButtons)
            {
                speedButton.UpdateView(speedButton.Speed == _speedController.CurrentSpeed);
            }
        }

        private void OnClick(int speed)
        {
            _speedController.SetSpeed(speed);
        }
    }
}