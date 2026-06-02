using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public abstract class EntityView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ClickData> ClickAction;
        
        protected SpriteOrderOptions _spriteOrderOptions;
        protected Vector2Int _position;
        
        public abstract IEntityData EntityData { get; }
        
        public virtual void Init(IEntityData entityData, SpriteOrderOptions spriteOrderOptions, IPalette specialPalette, Field field, Vector2Int position)
        {
            EntityData.DestroyRequest += OnDestroyRequest;
            EntityData.SetColor += OnSetColor;
            
            _spriteOrderOptions = spriteOrderOptions;
            _position = position;
            transform.position = field.GetPoint(_position);
        }

        protected abstract void OnSetColor(bool color);

        protected virtual void OnDestroyRequest(IEntityData data)
        {
            Destroy(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.Click));

        public void OnPointerEnter(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.HoverStart));

        public void OnPointerExit(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.HoverEnd));

        private ClickData GetClickData(PointerEventData eventData, InteractionType type) => new ClickData(type, eventData, EntityData);
    }
}