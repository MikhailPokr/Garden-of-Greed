using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public abstract class EntityView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ClickData> ClickAction;
        
        private Vector2Int _position;
        
        protected VisualContext _context;
        
        public abstract IEntityData EntityData { get; }
        
        public virtual void Init(IEntityData entityData, VisualContext context, Vector2Int position)
        {
            EntityData.DestroyRequest += OnDestroyRequest;
            
            _context = context;
            
            _position = position;
            transform.position = context.Field.GetPoint(_position);
        }

        protected virtual void OnDestroyRequest(IEntityData data)
        {
            Destroy(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.Click));

        public void OnPointerEnter(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.HoverStart));

        public void OnPointerExit(PointerEventData eventData) => ClickAction?.Invoke(GetClickData(eventData, InteractionType.HoverEnd));

        private ClickData GetClickData(PointerEventData eventData, InteractionType type) => new ClickData(type, eventData, EntityData);
        
        protected static Color ApplyDeviation(Color baseColor, float offset)
        {
            Color.RGBToHSV(baseColor, out float h, out float s, out float v);
    
            h += offset;
    
            h = Mathf.Repeat(h, 1f); 
    
            return Color.HSVToRGB(h, s, v);
        }
    }
}