using UnityEngine;

namespace Garden
{
    public struct FieldClickSignal : IClickSignal
    {
        public InteractionType InteractionType { get; }
        public readonly Vector2Int Position;

        public FieldClickSignal(InteractionType interactionType, Vector2Int position)
        {
            InteractionType = interactionType;
            Position = position;
        }
    }
}