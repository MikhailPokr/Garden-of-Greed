using System;
using PrimeTween;
using UnityEngine;

namespace Garden
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _cellWidth;
        [SerializeField] private float _rowHeight;
        [SerializeField] private Vector3 _center;
        
        private FieldManager _fieldManager;
        private Sequence _colorSequence;
        

        public void Init(FieldManager fieldManager)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            
            _fieldManager = fieldManager;
            
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorChanged;
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
                ? _fieldManager.GeneralPalette.NormalColor 
                : _fieldManager.GeneralPalette.NoColor;

            if (_spriteRenderer.color != targetColor)
            {
                _colorSequence.Group(Tween.Color(_spriteRenderer, targetColor, duration));
            }
        }
        

        public Vector3 GetPoint(Vector2Int position)
        {
            float xOffset = (position.y & 1) * (_cellWidth / 2f);

            float worldX = (position.x * _cellWidth) + xOffset;
            float worldY = -position.y * _rowHeight;

            return _center + new Vector3(worldX, worldY, 0);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for (int i = -10; i <= 10; i++)
            {
                for (int j = -10; j <= 10; j++)
                {
                    Gizmos.DrawSphere(GetPoint(new(i,j)), 0.1f);
                }
            }
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_center, 0.1f);
        }
    }
}