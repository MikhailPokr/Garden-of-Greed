using UnityEngine;

namespace Garden
{
    public struct EntityCreationSignal : ISignal
    {
        public readonly IEntityData EntityData;

        public EntityCreationSignal(IEntityData entityData)
        {
            EntityData = entityData;
        }
    }
}