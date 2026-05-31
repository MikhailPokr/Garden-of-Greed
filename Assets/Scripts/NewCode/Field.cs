using System;
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

        public void Init(FieldManager fieldManager)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            
            _fieldManager = fieldManager;
            
            fieldManager.ColorChanged += OnColorChanged;
        }

        private void OnColorChanged(float level)
        {
            _spriteRenderer.color = Color.Lerp(_fieldManager.GeneralPalette.NormalColor, _fieldManager.GeneralPalette.EvilColor, level);
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