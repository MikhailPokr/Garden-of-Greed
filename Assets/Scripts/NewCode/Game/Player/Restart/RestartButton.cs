using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Garden
{
    public class RestartButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _frame;
        [SerializeField] private Color _warmingColor;
        [SerializeField] private float _duration;
        private RestartController _restartController;
        private SpeedController _speedController;

        private bool _isActive;
        private Tween _tween;
        private float _time;

        public void Init(RestartController controller, SpeedController speedController)
        {
            _restartController = controller;
            _background.color = Color.white;

            _speedController = speedController;
            speedController.SpeedChange += () => SetActiveView(_speedController.CurrentSpeed == 0);

            SetActiveView(_speedController.CurrentSpeed == 0);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isActive)
                return;
            StartProcess();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InterruptProcess();
        }

        private void SetActiveView(bool active)
        {
            _isActive = active;
            _frame.color = _isActive ? Color.white : Color.gray2;
            InterruptProcess();
        }

        private void StartProcess()
        {
            InterruptProcess();

            _tween = Tween.Custom(this, startValue: 0f, endValue: 1f, duration: _duration,
                    onValueChange: (target, value) =>
                    {
                        target._background.color = Color.Lerp(Color.white, _warmingColor, value);
                    })
                    .OnComplete(this, target =>
                    {
                        target._restartController.Restart();
                    });
        }
        
        private void InterruptProcess()
        {
            if (_tween.isAlive)
            {
                _tween.Stop();
                _background.color = Color.white;
            }
        }
    }
}