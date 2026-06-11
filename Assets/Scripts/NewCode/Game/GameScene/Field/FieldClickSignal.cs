using UnityEngine;

namespace Garden
{
    public class FieldClickSignal : ISignal
    {
        public InteractionType InteractionType; 
        public Vector2Int  Position;

        public FieldClickSignal(InteractionType interactionType, Vector2Int position)
        {
            InteractionType = interactionType;
            Position = position;
        }
    }
}