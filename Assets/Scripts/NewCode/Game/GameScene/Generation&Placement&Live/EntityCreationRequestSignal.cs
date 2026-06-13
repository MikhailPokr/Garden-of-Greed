using UnityEngine;

namespace Garden
{
    public struct EntityCreationRequestSignal : ISignal
    {
        public IEntityData EntityData;
        public Vector2Int Position;

        public EntityCreationRequestSignal(IEntityData entityData, Vector2Int position)
        {
            EntityData = entityData;
            Position = position;
        }
    }
}