using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Garden
{
    public class UISpeedButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private Image _background;
        [Space]
        [SerializeField] private int _speed;
        public int Speed => _speed;
        [SerializeField] private Color _selectedColor;
        [Space]
        [SerializeField] private float _duration;
        [SerializeField] private float _scaleCoefficient;
        
        private SpeedController _speedController;

        private Vector3 _scale;
        private Sequence _scaleSequence;
        
        private bool _isSelected;
        private bool _isHover;
        private bool _isColored => _isSelected || _isHover;
        
        public event Action<int> OnClick;

        public void Init()
        {
            _scale = transform.localScale;
            UpdateView();
        }

        public void UpdateView(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateView();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(_speed);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHover = false;
            ChangeScale();
            UpdateView();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHover = true;
            ChangeScale();
            UpdateView();
        }
        
        private void ChangeScale()
        {
            if (_scaleSequence.isAlive)
                _scaleSequence.Stop();
            _scaleSequence = Sequence.Create();
            
            var targetScale = _isHover ? _scale * _scaleCoefficient : _scale;
            
            if (transform.localScale.x != targetScale.x)
                _scaleSequence.Group(Tween.Scale(transform, targetScale, _duration));
            
        }

        private void UpdateView()
        {
            if (_isColored)
                _background.color = _selectedColor;
            else
                _background.color = Color.white;
        }
    }
}