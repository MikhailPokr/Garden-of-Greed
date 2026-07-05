using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct BerryGrowingSignal : ISignal
    {
        public List<Vector2Int> Positions { get; }

        public BerryGrowingSignal(List<Vector2Int> positions)
        {
            Positions = positions;
        }
    }
}