using UnityEngine;

namespace Garden
{
    public struct EntityCreationRequestSignal : ISignal
    {
        public EntityType EntityType;
        public IEntityData EntityData;
        public Vector2Int Position;
        public int? Seed;
        public Transform Parent;

        public EntityCreationRequestSignal(EntityType entityType, IEntityData entityData, Vector2Int position)
        {
            EntityType = entityType;
            EntityData = entityData;
            Position = position;
            Seed = null;
            Parent = null;
        }
        public EntityCreationRequestSignal(EntityType entityType, Vector2Int position)
        {
            EntityType = entityType;
            EntityData = null;
            Position = position;
            Seed = null;
            Parent = null;
        }
        public EntityCreationRequestSignal(EntityType entityType, int seed, Vector2Int position)
        {
            EntityType = entityType;
            EntityData = null;
            Position = position;
            Seed = seed;
            Parent = null;
        }
    }
}