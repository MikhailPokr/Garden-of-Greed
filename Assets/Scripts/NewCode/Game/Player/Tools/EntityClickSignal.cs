namespace Garden
{
    public struct EntityClickSignal : IClickSignal
    {
        public EntityView Entity { get; }
        public InteractionType InteractionType { get; }
        public bool FieldSource { get; }

        public EntityClickSignal(EntityView entity, InteractionType interactionType, bool fieldSource)
        {
            Entity = entity;
            InteractionType = interactionType;
            FieldSource = fieldSource;
        }

        
    }
}