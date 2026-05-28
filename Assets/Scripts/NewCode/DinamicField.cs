using System;
using UnityEngine;

namespace Garden
{
    public class DinamicField : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public event Action ScreenSizeChanged;
        
        private Vector2Int _lastScreenSize;
        private FieldManager _fieldManager;

        public void Init(FieldManager fieldManager)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            
            _fieldManager = fieldManager;
            
            _spriteRenderer.sprite = _fieldManager.Palette.FieldSprite;
            
            fieldManager.ColorChanged += OnColorChanged;

            FitToScreenWidth();
        }

        private void OnColorChanged(float level)
        {
            _spriteRenderer.color = Color.Lerp(_fieldManager.Palette.NormalColor, _fieldManager.Palette.EvilColor, level);
        }

        public void Check()
        {
            if (Screen.width != _lastScreenSize.x || Screen.height != _lastScreenSize.y)
            {
                FitToScreenWidth();
            }
        }
        
        private void FitToScreenWidth()
        {
            float worldScreenHeight = _fieldManager.Camera.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight * _fieldManager.Camera.aspect;

            Vector2 newSize = _spriteRenderer.size;
            newSize.x = worldScreenWidth;
            _spriteRenderer.size = newSize;

            _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            
            ScreenSizeChanged?.Invoke();
        }
    }
}