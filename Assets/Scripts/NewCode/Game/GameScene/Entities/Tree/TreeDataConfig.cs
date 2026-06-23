using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct TreeDataConfig
    {
        public TreeGenomeConfig TreeGenomeConfig;
        public float TimerStart;
        
        public readonly float GetNextTimer(int stage) => TimerStart + TreeGenomeConfig.StageTime * (stage + 1);

        public readonly float GetCost(int stage)
        {
            if (stage < TreeGenomeConfig.LastGrowthStage)
            {
                return 0;
            }
            if (stage == Mathf.RoundToInt(TreeGenomeConfig.MaxStage))
            {
                return TreeGenomeConfig.WoodCostDry;
            }
            return TreeGenomeConfig.WoodCost;
        }
    }
}