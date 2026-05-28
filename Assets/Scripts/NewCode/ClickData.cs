using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class ClickData
    {
        public InteractionType InteractionType { get; private set; }
        public PointerEventData PointerEventData { get; private set; }
        public Collider2D Collider2D { get; private set; }
        public IEntityData EntityData { get; private set; }
        
        public ClickData(InteractionType interactionType, PointerEventData pointerEventData, Collider2D collider2D, IEntityData entityData)
        {
            InteractionType = interactionType;
            PointerEventData = pointerEventData;
            Collider2D = collider2D;
            EntityData = entityData;
        }
    }
}