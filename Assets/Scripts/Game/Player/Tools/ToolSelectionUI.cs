using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Garden
{
    public class ToolSelectionUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ToolType _toolType;
        public ToolType ToolType => _toolType;
        [Space]
        [SerializeField] private Image _background;
        [SerializeField] private Image _symbol;
        [SerializeField] private Image _frame;
        [Space] 
        [SerializeField] private float _scaleCoefficient;
        [SerializeField] private float _duration;
        private Vector3 _scale;
        
        private Color _colorSelected;
        private Color _backgroundColorNormal;
        private Color _backgroundColorSelected;
        
        private Sequence _scaleSequence;
        
        private bool _locked;
        private bool _isSelected;
        private bool _isHover;
        private bool _isColored => _isSelected || _isHover;

        public event Action<ToolType> OnClick;

        public void Init(Color color, Color backgroundColorNormal, float tValue)
        {
            _scaleSequence = new Sequence();
            _scale = transform.localScale;
            _colorSelected = color;
            _backgroundColorNormal = backgroundColorNormal;
            _backgroundColorSelected = Color.Lerp(_backgroundColorNormal, _colorSelected, tValue);
        }
        public void Activate(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateView();
        }

        public void Lock(bool locked)
        {
            _locked = locked;
            if (_locked)
                _frame.color = Color.gray2;
            else
                _frame.color = Color.white;
            OnPointerExit(null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_locked)
                return;
            OnClick?.Invoke(_toolType);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_locked)
                return;
            _isHover = true;
            UpdateView();
            ChangeScale();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_locked)
                return;
            _isHover = false;
            UpdateView();
            ChangeScale();
        }

        private void UpdateView()
        {
            _background.color = _isColored ? _backgroundColorSelected : _backgroundColorNormal;
            _symbol.color = _isColored ? _colorSelected : Color.white;
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
    }
}
