using System;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Garden
{
    public class Field : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private FieldOptions _fieldOptions;
        
        private Camera _mainCamera;
        private GeneralPalette _generalPalette;
        private IGridMath _gridMath;
        
        private Sequence _colorSequence;

        private Vector2Int _lastPosition;
        public Vector2Int CurrentHoverPosition => _lastPosition;
        
        private bool _isPointerOverField;
        private readonly Vector2Int _invalidPosition = new Vector2Int(int.MinValue, int.MinValue);

        public void Init(IGridMath gridMath, GeneralPalette palette, FieldOptions fieldOptions)
        {
            _fieldOptions = fieldOptions;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mainCamera = Camera.main;
            _generalPalette = palette;
            _gridMath = gridMath;
            
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorChanged;
            
            _lastPosition = _invalidPosition;
        }

        private void OnColorChanged(ColorModeChangedSignal signal)
        {
            PlayColorSequence(signal.IsColored);
        }
        
        private void PlayColorSequence(bool toFullColor)
        {
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
    
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetColor = toFullColor 
                ? _generalPalette.NormalColor 
                : _generalPalette.NoColor;

            if (_spriteRenderer.color != targetColor)
            {
                _colorSequence.Group(Tween.Color(_spriteRenderer, targetColor, duration));
            }
        }

        public void OnPointerClick(PointerEventData eventData) => 
            SignalBus<FieldClickSignal>.Fire(new FieldClickSignal(InteractionType.Click, 
                _gridMath.GetPosition(_mainCamera.ScreenToWorldPoint(eventData.position))));
        
        private void OnDrawGizmos()
        {
            if (_gridMath == null)
                return;
            
            
            Gizmos.color = Color.yellow;
            for (int i = _fieldOptions.Bounds.xMin; i < _fieldOptions.Bounds.xMax; i++)
            {
                for (int j = _fieldOptions.Bounds.yMin; j < _fieldOptions.Bounds.yMax; j++)
                {
                    var pos = new Vector2Int(i, j);
                    Gizmos.DrawWireSphere(_gridMath.GetPoint(pos), 0.15f);
                    UnityEditor.Handles.Label( _gridMath.GetPoint(pos), pos.ToString());
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOverField = true;
            ProcessPointerPosition(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOverField = false;
    
            if (_lastPosition != _invalidPosition)
            {
                SignalBus<FieldClickSignal>.Fire(new FieldClickSignal(InteractionType.HoverEnd, _lastPosition));
                _lastPosition = _invalidPosition; 
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_isPointerOverField) return;
            ProcessPointerPosition(eventData);
        }
        
        private void ProcessPointerPosition(PointerEventData eventData)
        {
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            Vector2Int position = _gridMath.GetPosition(worldPosition);

            if (position != _lastPosition)
            {
                if (_lastPosition != _invalidPosition)
                {
                    SignalBus<FieldClickSignal>.Fire(new FieldClickSignal(InteractionType.HoverEnd, _lastPosition));
                }

                SignalBus<FieldClickSignal>.Fire(new FieldClickSignal(InteractionType.HoverStart, position));
            }
            _lastPosition = position;
        }
    }
}