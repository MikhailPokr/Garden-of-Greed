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
        public int LastGrowthStage;
        public int GrownSpriteIndex;
        
        public float StageTime;
        public int MaxStage;
        
        public int WoodCost;
        public int WoodCostDry;
        
        public int LastFruitStage;
        public int BaseFruitCost;
        
        public readonly float GetNextTimer(int stage) => TimerStart + StageTime * stage;
        public readonly int GetCost(int stage) => stage == MaxStage ? WoodCostDry : WoodCost;
    }
}