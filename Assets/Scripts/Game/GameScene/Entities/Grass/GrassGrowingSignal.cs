using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct GrassGrowingSignal : ISignal
    {
        public List<Vector2Int> Positions { get; }

        public GrassGrowingSignal(List<Vector2Int> positions)
        {
            Positions = positions;
        }
    }
}