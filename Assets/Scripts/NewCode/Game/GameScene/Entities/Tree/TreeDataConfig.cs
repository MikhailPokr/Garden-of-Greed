using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct TreeDataConfig
    {
        public TreeGenomeConfig TreeGenomeConfig;
        public float TimerStart;
        public float DeadValue;
        public GeoMap GeoMap;
        
        public readonly float GetNextTimer(int stage, Vector2Int position) => TimerStart + TreeGenomeConfig.StageTime * (stage + 1) + GeoMap.Map[position] * TreeGenomeConfig.PenaltyPerPoint;
    }
}