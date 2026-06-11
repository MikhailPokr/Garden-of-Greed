using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct TreeDataConfig
    {
        public TreeGenomeConfig TreeGenomeConfig;
        public float TimerStart;
        
        public readonly float GetNextTimer(int stage) => TimerStart + TreeGenomeConfig.StageTime * (stage + 1);
        public readonly float GetCost(int stage) => stage == Mathf.RoundToInt(TreeGenomeConfig.MaxStage) ?
            TreeGenomeConfig.WoodCostDry : TreeGenomeConfig.WoodCost;
    }
}