using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public abstract class EntityView : MonoBehaviour
    {
        public event Action<EntityView, InteractionType> ClickAction;
        
        protected VisualContext _context;
        
        public abstract IEntityData EntityData { get; }
        public abstract EntityType EntityType { get; }
        
        public virtual void Init(IEntityData entityData, VisualContext context)
        {
            EntityData.DestroyRequest += OnDestroyRequest;
            
            _context = context;

            if (entityData.Position == null)
                throw new Exception("Incorrect creation order");
            transform.position = context.SpatialMap.GetPoint((Vector2Int)entityData.Position);

            SignalBus<FieldClickSignal>.OnEvent += (signal) => 
            {
                if (signal.Position != entityData.Position)
                    return;
                OnInteract(signal.InteractionType);
            };
        }

        protected virtual void OnInteract(InteractionType type)
        {
            ClickAction?.Invoke(this, type);
        }

        protected virtual void OnDestroyRequest(IEntityData data)
        {
            Destroy(gameObject);
        }
        
        protected static Color ApplyDeviation(Color baseColor, float offset)
        {
            Color.RGBToHSV(baseColor, out float h, out float s, out float v);
    
            h += offset;
    
            h = Mathf.Repeat(h, 1f); 
    
            return Color.HSVToRGB(h, s, v);
        }
    }
}