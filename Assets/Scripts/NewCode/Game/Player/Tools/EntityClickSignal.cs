namespace Garden
{
    public struct EntityClickSignal : IClickSignal
    {
        public readonly EntityView Entity;
        public InteractionType InteractionType { get; }

        public EntityClickSignal(EntityView entity, InteractionType interactionType)
        {
            Entity = entity;
            InteractionType = interactionType;
        }

        
    }
}