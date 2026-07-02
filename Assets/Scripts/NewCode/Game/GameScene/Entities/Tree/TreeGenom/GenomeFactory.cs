using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class GenomeFactory
    {
        private readonly TreeGenerationOptions _treeOptions;
        private readonly FruitGenerationOptions _fruitOptions;
        
        private ITreePalette _treePalette;
        private IFruitPalette _fruitPalette;
        
        private readonly MutationOptions _mutationOptions;

        public GenomeFactory(EntityBundle treeBundle, EntityBundle fruitBundle, MutationOptions mutationOptions)
        {
            _treeOptions = (TreeGenerationOptions)treeBundle.GenerationOptions;
            _fruitOptions = (FruitGenerationOptions)fruitBundle.GenerationOptions;
            _treePalette = (ITreePalette)treeBundle.Palette;
            _fruitPalette = (IFruitPalette)fruitBundle.Palette;
            _mutationOptions = mutationOptions;
        }

        public TreeGenomeConfig Create(int seed)
        {
            TreeType treeType = GenerateType(seed);
            
            var lastGrowthStage = SeedUtils.GetRandom(seed, ParamType.LastGrowthStage, _treeOptions.GetGrowthLastStageRange(treeType));
            lastGrowthStage = Math.Clamp(lastGrowthStage, 0, _treePalette.GetStageSpritesCount());
            var sprite = SeedUtils.GetRandom(seed, ParamType.TreeSprite, _treePalette.GetSpritesCount(treeType));
            var greenOffset = SeedUtils.GetRandom(seed, ParamType.GreenColorOffset, _treeOptions.GetGreenOffsetRange());
            
            var stageTime = SeedUtils.GetRandom(seed, ParamType.StageTime, _treeOptions.GetStageTimeRange(treeType));
            var penaltyPerPoint = SeedUtils.GetRandom(seed, ParamType.PenaltyPerPoint, _treeOptions.GetPenaltyPerPointRange());
            var woodCost = SeedUtils.GetRandom(seed, ParamType.WoodCost, _treeOptions.GetWoodCostRange(treeType));
            var dryWoodCost = SeedUtils.GetRandom(seed, ParamType.DryWoodCost, _treeOptions.GetDryWoodCostRange());
            var woodColor = SeedUtils.GetRandom(seed, ParamType.WoodColor, _treePalette.GetWoodColorsCount(treeType));
            
            var maxStage = lastGrowthStage + 1 + SeedUtils.GetRandom(seed, ParamType.MaxStage, _treeOptions.GetMaxStageRange(treeType));
            
            var lastFruitStage = 0;
            
            var fruitCostMultiplier = SeedUtils.GetRandom(seed, ParamType.CostMultiplier, _fruitOptions.GatFruitCostMultiplierRange(treeType));
            var startQuality = SeedUtils.GetRandom(seed, ParamType.StartQuality, _fruitOptions.GetStartQualityRange(treeType));
            var fruitRotingTime = SeedUtils.GetRandom(seed, ParamType.RotingTime, _fruitOptions.GetRottingTimeRange(treeType));
            var fruitSpriteIndex = SeedUtils.GetRandom(seed, ParamType.FruitSprite, _fruitPalette.GetSpritesCount(treeType));
            var fruitColor = SeedUtils.GetRandom(seed, ParamType.FruitColor, _fruitPalette.GetColorsCount(treeType));
            
            if (treeType.HasFlag(TreeType.Fruit))
            {
                lastFruitStage = SeedUtils.GetRandom(seed, ParamType.LastFruitStage, _treeOptions.GetLastFruitStageRange(treeType));
                lastFruitStage = Math.Clamp(lastFruitStage, lastGrowthStage + 1, maxStage - 1);
            }
            
            var genome = new TreeGenomeConfig()
            {
                RootSeed = seed,
                Seed = seed,
                Indexes = new List<int>(),
                
                TreeType = treeType,
                GrownSpriteIndex = sprite,
                GreenOffset = greenOffset,
                LastGrowthStage = lastGrowthStage,
                WoodColorIndex = woodColor,
                
                LastFruitStage = lastFruitStage,
                
                StageTime = stageTime,
                PenaltyPerPoint = penaltyPerPoint,
                MaxStage = maxStage,
                WoodCost = woodCost,
                WoodCostDry = dryWoodCost,
                
                Quality = startQuality,
                FruitCostMultiplier = fruitCostMultiplier,
                FruitRotingTime = fruitRotingTime,
                FruitSpriteIndex = fruitSpriteIndex,
                FruitColorIndex = fruitColor,
            };
            
            return genome;
        }

        public int GetSeed(int seed, int index) => SeedUtils.GetNewSeed(seed, index);

        public TreeGenomeConfig Mutate(TreeGenomeConfig treeGenomeConfig, int childIndex)
        {
            int childSeed = GetSeed(treeGenomeConfig.Seed, childIndex);
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
                PenaltyPerPoint =treeGenomeConfig.PenaltyPerPoint,
                WoodColorIndex =  treeGenomeConfig.WoodColorIndex,
                WoodCostDry = treeGenomeConfig.WoodCostDry,
                Quality = treeGenomeConfig.Quality,
                FruitRotingTime = treeGenomeConfig.FruitRotingTime,
                FruitCostMultiplier = treeGenomeConfig.FruitCostMultiplier,
                FruitSpriteIndex = treeGenomeConfig.FruitSpriteIndex,
                FruitColorIndex = treeGenomeConfig.FruitColorIndex,
                
                StageTime = GetRandomMutation(childSeed, treeGenomeConfig.TreeType, ParamType.StageTime, treeGenomeConfig.StageTime),
                MaxStage = treeGenomeConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod, 2),
                LastFruitStage = treeGenomeConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod - 1, 1),
                WoodCost = GetRandomMutation(childSeed, treeGenomeConfig.TreeType, ParamType.WoodCost, treeGenomeConfig.WoodCost),
            };
            
            return config;
        }
        
        public TreeGenomeConfig MutateWithQuality(TreeGenomeConfig treeGenomeConfig, int fruitIndex)
        {
            int childSeed = GetSeed(treeGenomeConfig.Seed, fruitIndex);
            List<int> Indexes = new List<int>(treeGenomeConfig.Indexes);
            Indexes.Add(fruitIndex);

            var quality = treeGenomeConfig.Quality;
            
            float adultPeriod = treeGenomeConfig.MaxStage - treeGenomeConfig.LastGrowthStage;

            int mutatedAdultPeriod = Mathf.RoundToInt(MutateValue(childSeed, ParamType.MaxStage, adultPeriod, ref quality));
            var stageTime = MutateValue(childSeed, ParamType.StageTime, treeGenomeConfig.StageTime, ref quality);
            var woodCost = MutateValue(childSeed, ParamType.WoodCost, treeGenomeConfig.WoodCost, ref quality);
            var fruitRotingTime = MutateValue(childSeed, ParamType.RotingTime, treeGenomeConfig.FruitRotingTime, ref quality);
            var fruitCostMultiplier = MutateValue(childSeed, ParamType.CostMultiplier, treeGenomeConfig.FruitCostMultiplier, ref quality);
            
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
                FruitSpriteIndex =  treeGenomeConfig.FruitSpriteIndex,
                FruitColorIndex = treeGenomeConfig.FruitColorIndex,
                
                FruitCostMultiplier = fruitCostMultiplier,
                FruitRotingTime =  fruitRotingTime,
                
                StageTime = stageTime,
                MaxStage = treeGenomeConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod, 2),
                LastFruitStage = treeGenomeConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod - 1, 1),
                WoodCost = woodCost,
                
                Quality = quality,
            };
            
            return config;
        }
        
        private TreeType GenerateType(int seed)
        {
            List<TreeTypeConfig> chances = _treeOptions.GetChances();
            TreeType treeType = 0;
            foreach (var treeTypeConfig in chances)
            {
                if (SeedUtils.GetRandom(seed, treeTypeConfig.ParamType, new Vector2(0, 1)) < treeTypeConfig.Chance)
                    treeType |= treeTypeConfig.TreeType;
            }
            return treeType;
        }
        
        private float GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, float origin)
        {
            int mutationPercent = SeedUtils.GetRandom(childSeed, paramType, _mutationOptions.GetAutoBreedMutationPercentRange(treeType));
    
            float multiplier = 100f + mutationPercent;
            return origin * (multiplier / 100f);
        }

        private float MutateValue(int childSeed, ParamType paramType, float origin, ref int mutationValue)
        {
            mutationValue -= SeedUtils.GetRandom(childSeed, paramType, new Vector2Int(-1, 1));
            
            float multiplier = 100f + mutationValue * _mutationOptions.GetFruitMutationPercent();
            return origin * (multiplier / 100f);
        }
    }
}