using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class ClickData
    {
        public InteractionType InteractionType { get; private set; }
        public PointerEventData PointerEventData { get; private set; }
        public IEntityData EntityData { get; private set; }
        
        public ClickData(InteractionType interactionType, PointerEventData pointerEventData, IEntityData entityData)
        {
            InteractionType = interactionType;
            PointerEventData = pointerEventData;
            EntityData = entityData;
        }
    }
}