using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public struct TreeDataConfig
    {
        public List<Sprite> StagesSprites;
        public Sprite DieSprite;
        
        public float StageTime;
        public int MaxStage;
        
        public bool AutoBreedingTree;
        public int BreedCount;
        
        public int WoodCost;
        public int WoodCostDry;
        
        public float TimerStart;
        
        public bool IsFruitTree;
        public Sprite FruitSprite;
        public int MaxFruit;
        public int LastFruitStage;
        public int BaseFruitCost;

        public bool IsEvilTree;
        
        
        public readonly float GetNextTimer(int stage) => TimerStart + StageTime * stage;
        public readonly int GetCost(int stage) => stage == MaxStage ? WoodCostDry : WoodCost;
    }
}