using System;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Garden
{
    public class Field : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private FieldOptions _fieldOptions;
        
        private Camera _mainCamera;
        private GeneralPalette _generalPalette;
        private IGridMath _gridMath;
        
        private Sequence _colorSequence;

        private InputAction _pointer;

        private Vector2Int _lastPosition;

        public void Init(IGridMath gridMath, GeneralPalette palette, FieldOptions fieldOptions)
        {
            _fieldOptions = fieldOptions;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mainCamera = Camera.main;
            _generalPalette = palette;
            _gridMath = gridMath;
            
            _pointer = InputSystem.actions.FindAction("Point");
            
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorChanged;
        }

        public void UpdateLogic()
        {
            Vector2Int position = _gridMath.GetPosition(_mainCamera.ScreenToWorldPoint(_pointer.ReadValue<Vector2>()));
            if (position != _lastPosition)
            {
                SignalBus<FieldClickSignal>.Fire(new FieldClickSignal(InteractionType.HoverEnd, _lastPosition));
                SignalBus<FieldClickSignal>.Fire(new FieldClickSignal(InteractionType.HoverStart, position));
            }
            _lastPosition = position;
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
    }
}