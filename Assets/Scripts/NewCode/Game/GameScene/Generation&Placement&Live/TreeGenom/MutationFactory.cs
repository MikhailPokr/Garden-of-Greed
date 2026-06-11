using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class MutationFactory
    {
        private readonly MutationOptions _options;
        
        public MutationFactory(MutationOptions options)
        {
            _options = options;
        }

        public TreeGenomeConfig Create(TreeGenomeConfig treeGenomeConfig, int childIndex)
        {
            int childSeed = SeedUtils.GetNewSeed(treeGenomeConfig.Seed, childIndex);
            List<int> Indexes = new List<int>(treeGenomeConfig.Indexes);
            Indexes.Add(childIndex);
            
            float adultPeriod = treeGenomeConfig.MaxStage - treeGenomeConfig.LastGrowthStage;
            int mutatedAdultPeriod = Mathf.RoundToInt(GetRandomMutation(childSeed, treeGenomeConfig.TreeType, ParamType.MaxStage, adultPeriod));
            
            var config = new TreeGenomeConfig
            {
                RootSeed = treeGenomeConfig.RootSeed,
                Seed = childSeed,
                Indexes = Indexes,
                
                TreeType = treeGenomeConfig.TreeType,
                GrownSpriteIndex = treeGenomeConfig.GrownSpriteIndex,
                GreenOffset = treeGenomeConfig.GreenOffset,
                LastGrowthStage = treeGenomeConfig.LastGrowthStage,
                WoodColorIndex =  treeGenomeConfig.WoodColorIndex,
                
                
                WoodCostDry = treeGenomeConfig.WoodCostDry,
                StageTime = GetRandomMutation(childSeed, treeGenomeConfig.TreeType, ParamType.StageTime, treeGenomeConfig.StageTime),
                MaxStage = treeGenomeConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod, 2),
                LastFruitStage = treeGenomeConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod - 1, 1),
                WoodCost = GetRandomMutation(childSeed, treeGenomeConfig.TreeType, ParamType.WoodCost, treeGenomeConfig.WoodCost),
            };
            
            return config;
        }
        
        public TreeGenomeConfig Create(FruitData fruitData)
        {
            throw new NotImplementedException();
        }
        
        private float GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, float origin)
        {
            int mutationPercent = SeedUtils.GetRandom(childSeed, paramType, _options.GetAutoBreedMutationPercentRange(treeType));
    
            float multiplier = 100f + mutationPercent;
            return origin * (multiplier / 100f);
        }
    }
}