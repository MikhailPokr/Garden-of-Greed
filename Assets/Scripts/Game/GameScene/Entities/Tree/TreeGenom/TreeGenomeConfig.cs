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
        public float WoodCostDry;
        public float FuelForce;
        
        public float StageTime;
        public float PenaltyPerPoint;
        public float MaxStage;
        public float LastFruitStage;

        public int Quality;
        
        public int FruitSpriteIndex;
        public int FruitColorIndex;
        public float FruitCostMultiplier;
        public float FruitRotingTime;
        public float FruitLifeRegeneration;
    }
}