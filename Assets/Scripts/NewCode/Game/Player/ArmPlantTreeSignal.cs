using UnityEngine;

namespace Garden
{
    public struct ArmPlantTreeSignal : ISignal
    {
        public readonly int Seed;
        public readonly Vector2Int Position;
        public ArmPlantTreeSignal(int seed, Vector2Int position)
        {
            Seed = seed;
            Position = position;
        }
    }
}