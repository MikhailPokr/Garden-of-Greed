using UnityEngine;

namespace Garden
{
    public struct EntityCreationRequestSignal : ISignal
    {
        public EntityType EntityType;
        public IEntityData EntityData;
        public Vector2Int IntPosition;
        public Vector2 Position;

        public EntityCreationRequestSignal(
            EntityType entityType, IEntityData entityData, Vector2 position, Vector2Int intPosition)
        {
            EntityType = entityType;
            EntityData = entityData;
            Position = position;
            IntPosition = intPosition;
        }
    }
}