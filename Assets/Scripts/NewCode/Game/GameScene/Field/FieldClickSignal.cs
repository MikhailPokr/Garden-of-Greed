using UnityEngine;

namespace Garden
{
    public struct FieldClickSignal : IClickSignal
    {
        public InteractionType InteractionType { get; }
        public Vector2Int Position { get; }
        
        public FieldClickSignal(InteractionType interactionType, Vector2Int position)
        {
            InteractionType = interactionType;
            Position = position;
        }
    }
}