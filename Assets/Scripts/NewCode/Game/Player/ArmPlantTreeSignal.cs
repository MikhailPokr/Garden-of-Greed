using UnityEngine;

namespace Garden
{
    public struct ArmPlantTreeSignal : ISignal
    {
        public readonly EntityType Type;
        public readonly int Seed;
        public readonly Vector2Int Position;
        public ArmPlantTreeSignal(EntityType type, int seed, Vector2Int position)
        {
            Type = type;
            Seed = seed;
            Position = position;
        }
    }
}