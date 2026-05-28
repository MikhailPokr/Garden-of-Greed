using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class EntityView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ClickData> ClickAction;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        private IEntityData _entityData;
        private FieldManager _fieldManager;
        private DinamicField _dinamicField;

        private float _screenXRatio;
        
        public void Init(IEntityData entityData, FieldManager manager, DinamicField dinamicField, bool needChangeColor)
        {
            _entityData = entityData;
            _entityData.DestroyRequest += OnDestroyRequest;
            _entityData.ChangeSpriteRequest += OnChangeSpriteRequest;
            
            _fieldManager = manager;

            if (needChangeColor)
            {
                _fieldManager.ColorChanged += OnColorChanged;
                _entityData.WashColor += OnWashColor;
            }
            
            _dinamicField = dinamicField;
            
            Vector3 currentViewportPos = _fieldManager.Camera.WorldToViewportPoint(transform.position);
            _screenXRatio = currentViewportPos.x;
            
            _dinamicField.ScreenSizeChanged += AlignPositionX;
        }

        private void OnChangeSpriteRequest(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
            
            _boxCollider.size = _spriteRenderer.sprite.bounds.size;
            _boxCollider.offset = _spriteRenderer.sprite.bounds.center;
        }

        private void OnWashColor()
        {
            _spriteRenderer.color = Color.white;
            _fieldManager.ColorChanged -= OnColorChanged;
        }

        private void OnColorChanged(float level)
        {
            _spriteRenderer.color = Color.Lerp(_fieldManager.Palette.NormalColor, _fieldManager.Palette.EvilColor, level);
        }

        private void OnDestroyRequest(IEntityData data)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _entityData.DestroyRequest -= OnDestroyRequest;
            _entityData.ChangeSpriteRequest -= OnChangeSpriteRequest;
            _fieldManager.ColorChanged -= OnColorChanged;
            _entityData.WashColor -= OnWashColor;
            _dinamicField.ScreenSizeChanged -= AlignPositionX;
        }
        
        private void AlignPositionX()
        {
            float zDistance = Mathf.Abs(_fieldManager.Camera.transform.position.z - transform.position.z);
            
            Vector3 viewportPos = new Vector3(_screenXRatio, 0f, zDistance);
        
            Vector3 worldPos = _fieldManager.Camera.ViewportToWorldPoint(viewportPos);

            transform.position = new Vector3(worldPos.x, transform.position.y, transform.position.z);
        }

        public void OnPointerClick(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.Click));

        public void OnPointerEnter(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.HoverStart));

        public void OnPointerExit(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.HoverEnd));

        private ClickData GetClickData(PointerEventData eventData, InteractionType type) => new ClickData(type, eventData, _boxCollider, _entityData);
    }
}