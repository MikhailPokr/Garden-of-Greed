using System;
using PrimeTween;
using UnityEngine;

namespace Garden
{
    public abstract class EntityView : MonoBehaviour
    {
        protected VisualContext _context;
        protected Sequence _colorSequence;

        protected bool _selected;
        protected bool _colored;
        protected bool IsColored => _colored || _selected;
        
        public abstract IEntityData EntityData { get; }
        public abstract EntityType EntityType { get; }
        
        public virtual void Init(IEntityData entityData, VisualContext context)
        {
            EntityData.CommandRequest += OnCommand;
            _context = context;

            if (entityData.Position == null)
                throw new Exception("Incorrect creation order");
                
            transform.position = context.SpatialMap.GetPoint((Vector2Int)entityData.Position);
            
            _colored = context.Color;
            _selected = context.Field.CurrentHoverPosition == entityData.Position;

            SignalBus<FieldClickSignal>.OnEvent += OnFieldClickSignal;
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorModeChanged;
        }
        
        private void OnFieldClickSignal(FieldClickSignal signal)
        {
            if (signal.Position != EntityData.Position) return;
            OnInteract(signal.InteractionType);
        }

        protected virtual void OnInteract(InteractionType type)
        {
            _selected = type switch
            {
                InteractionType.HoverStart => true,
                InteractionType.HoverEnd => false,
                _ => _selected
            };
            
            PlayColorSequence(IsColored);
            SignalBus<EntityClickSignal>.Fire(new EntityClickSignal(this, type)); 
        }

        private void OnColorModeChanged(ColorModeChangedSignal signal)
        {
            _colored = signal.IsColored;
    
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
        
            PlayColorSequence(signal.IsColored);
        }

        protected abstract void PlayColorSequence(bool toFullColor);
        protected abstract void PlayBlinkSequence();

        protected virtual void OnCommand(ICommand[] commands)
        {
            foreach (var command in commands)
                switch (command)
                {
                    case DestroyCommand:
                        Destroy(gameObject);
                        break;
                }
        }
        
        protected static Color ApplyDeviation(Color baseColor, float offset)
        {
            Color.RGBToHSV(baseColor, out float h, out float s, out float v);
            h += offset;
            h = Mathf.Repeat(h, 1f); 
            return Color.HSVToRGB(h, s, v);
        }

        public virtual void SetEntity(EntityView entity)
        {
            entity.gameObject.transform.SetParent(transform);
            entity.gameObject.transform.localPosition = GetPosition();
        }

        protected virtual Vector2 GetPosition() => 
            _context.SpatialMap.GetPoint(EntityData.Position.Value);

        protected virtual void OnDestroy()
        {
            EntityData.CommandRequest -= OnCommand;
            SignalBus<FieldClickSignal>.OnEvent -= OnFieldClickSignal;
            SignalBus<ColorModeChangedSignal>.OnEvent -= OnColorModeChanged;
        }
    }
}