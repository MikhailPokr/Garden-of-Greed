using System.Collections.Generic;

namespace Garden
{
    public struct TreeGenomeConfig
    {
        public int RootSeed;
        public int Seed;
        public List<int> Indexes;
        
        public TreeType TreeType;
        public int GrownSpriteIndex;
        public float GreenOffset;
        public float LastGrowthStage;
        public int WoodColorIndex;
        
        public float WoodCost;
        
        public float LastFruitStage;
        
        public float StageTime;
        public float MaxStage;
        public float WoodCostDry;
    }
}