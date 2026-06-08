using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct TreeDataConfig
    {
        public int RootSeed;
        public int Seed;
        public List<int> Indexes;
        
        public float TimerStart;
        public TreeType TreeType;
        public float LastGrowthStage;
        public int GrownSpriteIndex;
        public float GreenOffset;
        
        public float StageTime;
        public float MaxStage;
        
        public float WoodCost;
        public float WoodCostDry;
        public int WoodColorIndex;
        
        public float LastFruitStage;
        public float BaseFruitCost;
        
        public readonly float GetNextTimer(int stage) => TimerStart + StageTime * (stage + 1);
        public readonly float GetCost(int stage) => stage == Mathf.RoundToInt(MaxStage) ? WoodCostDry : WoodCost;
    }
}