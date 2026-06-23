using UnityEngine;

namespace Garden
{
    public struct ArmPlantFruitSignal : ISignal
    {
        public readonly FruitData FruitData;
        public readonly Vector2Int Position;
        public ArmPlantFruitSignal(FruitData fruitData, Vector2Int position)
        {
            FruitData = fruitData;
            Position = position;
        }
    }
}