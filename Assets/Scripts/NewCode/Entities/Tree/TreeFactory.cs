using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Garden
{
    public class TreeFactory : IFactory
    {
        private readonly TreePalette _treePalette;
        private readonly TreeGenerationOptions _options;
        private readonly Player _player;

        private readonly int _seed;
        private int _seedUsages;
        
        public TreeFactory(int globalSeed, EntityBundle bundle, Player player)
        {
            _treePalette = (TreePalette)bundle.Palette;
            _options = (TreeGenerationOptions)bundle.Options;
            _player = player;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.TreeFactory);
            _seedUsages = 0;
        }
        
        public IEntityData Create(EntityCreationRequestSignal signal)
        {
            if (signal.EntityData != null)
                return signal.EntityData;
            if (signal.Seed != null)
                return Create((int)signal.Seed);
            return Create();
        }

        public List<IEntityData> Create(EntityView origin)
        {
            TreeData originTreeData = (TreeData)origin.EntityData;
            int count = SeedUtils.GetRandom(originTreeData.DataConfig.Seed, ParamType.AutoBreedCount,
                _options.GetAutoBreedCountRange(originTreeData.DataConfig.TreeType));
            
            List<IEntityData> newTreeData = new List<IEntityData>();
            
            for (int i = 0; i < count; i++)
            {
                newTreeData.Add(Create(originTreeData, i));
            }

            return newTreeData;
        }

        public TreeData Create() => Create(SeedUtils.GetNewSeed(_seed, _seedUsages++));
        
        public TreeData Create(int seed)
        {
            TreeType treeType = GenerateType(seed);
            
            var lastGrowthStage = SeedUtils.GetRandom(seed, ParamType.LastGrowthStage, _options.GetGrowthLastStageRange(treeType));
            lastGrowthStage = Math.Clamp(lastGrowthStage, 0, _treePalette.StageSprites.Count);
            var sprite = treeType switch
            {
                _ when treeType.HasFlag(TreeType.Evil) => SeedUtils.GetRandom(seed, ParamType.TreeSprite,
                    _treePalette.TreeEvilSprites.Count),
                _ => SeedUtils.GetRandom(seed, ParamType.TreeSprite, _treePalette.TreeSprites.Count)
            };
            var greenOffset = SeedUtils.GetRandom(seed, ParamType.GreenColorOffset, _options.GetGreenOffsetRange());
            
            var stageTime = SeedUtils.GetRandom(seed, ParamType.StageTime, _options.GetStageTimeRange(treeType));
            var woodCost = SeedUtils.GetRandom(seed, ParamType.WoodCost, _options.GetWoodCostRange(treeType));
            var dryWoodCost = SeedUtils.GetRandom(seed, ParamType.DryWoodCost, _options.GetDryWoodCostRange());
            var woodColor = SeedUtils.GetRandom(seed, ParamType.WoodColor, _treePalette.WoodColors.Count);
            
            var maxStage = lastGrowthStage + 1 + SeedUtils.GetRandom(seed, ParamType.MaxStage, _options.GetMaxStageRange(treeType));
            
            var lastFruitStage = 0;
            
            if (treeType.HasFlag(TreeType.Fruit))
            {
                lastFruitStage = SeedUtils.GetRandom(seed, ParamType.LastFruitStage, _options.GetLastFruitStageRange(treeType));
            }
            
            var config = new TreeDataConfig
            {
                RootSeed = seed,
                Seed = seed,
                Indexes = new List<int>(),
                TimerStart = _player.Time,
                TreeType = treeType,
                LastGrowthStage = lastGrowthStage,
                GrownSpriteIndex = sprite,
                GreenOffset = greenOffset,
                StageTime = stageTime,
                MaxStage = maxStage,
                WoodCost = woodCost,
                WoodCostDry = dryWoodCost,
                WoodColorIndex = woodColor,
                LastFruitStage = lastFruitStage
            };
            
            return new TreeData(config);
        }

        private TreeType GenerateType(int seed)
        {
            List<TreeTypeConfig> chances = _options.GetChances();
            TreeType treeType = 0;
            foreach (var treeTypeConfig in chances)
            {
                if (SeedUtils.GetRandom(seed, treeTypeConfig.ParamType, 100) < treeTypeConfig.ChanceInPercent)
                    treeType |= treeTypeConfig.TreeType;
            }
            return treeType;
        }

        public TreeData Create(TreeData data, int childIndex)
        {
            int childSeed = SeedUtils.GetNewSeed(data.DataConfig.Seed, childIndex);
            List<int> Indexes = new List<int>(data.DataConfig.Indexes);
            Indexes.Add(childIndex);
            
            float adultPeriod = data.DataConfig.MaxStage - data.DataConfig.LastGrowthStage;
            int mutatedAdultPeriod = Mathf.RoundToInt(GetRandomMutation(childSeed, data.DataConfig.TreeType, ParamType.MaxStage, adultPeriod));
            
            
            var config = new TreeDataConfig
            {
                RootSeed = data.DataConfig.RootSeed,
                Seed = childSeed,
                Indexes = Indexes,
                TimerStart = _player.Time,
                TreeType = data.DataConfig.TreeType,
                LastGrowthStage = data.DataConfig.LastGrowthStage,
                GreenOffset = data.DataConfig.GreenOffset,
                GrownSpriteIndex = data.DataConfig.GrownSpriteIndex,
                WoodCostDry = data.DataConfig.WoodCostDry,
                WoodColorIndex =  data.DataConfig.WoodColorIndex,
                LastFruitStage = data.DataConfig.LastFruitStage,
                
                StageTime = GetRandomMutation(childSeed, data.DataConfig.TreeType, ParamType.StageTime, data.DataConfig.StageTime),
                MaxStage = data.DataConfig.LastGrowthStage + Mathf.Max(mutatedAdultPeriod, 2),
                WoodCost = GetRandomMutation(childSeed, data.DataConfig.TreeType, ParamType.WoodCost, data.DataConfig.WoodCost),
            };
            
            return new TreeData(config);
        }
        public TreeData Create(FruitData data)
        {
            throw  new NotImplementedException();
        }
        
        private float GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, float origin)
        {
            int mutationPercent = SeedUtils.GetRandom(childSeed, paramType, _options.GetAutoBreedMutationPercentRange(treeType));
    
            float multiplier = 100f + mutationPercent;
            return origin * (multiplier / 100f);
        }

       
    }
}