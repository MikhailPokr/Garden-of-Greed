using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Garden
{
    public class TreeFactory : IFactory
    {
        private readonly FieldManager _fieldManager;
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

        IEntityData IFactory.Create() => Create();
        IEntityData IFactory.Create(int seed) => Create(seed);

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
            
            var stageTime = SeedUtils.GetRandom(seed, ParamType.StageTime, _options.GetStageTimeRange(treeType));
            var woodCost = SeedUtils.GetRandom(seed, ParamType.WoodCost, _options.GetWoodCostRange(treeType));
            var dryWoodCost = SeedUtils.GetRandom(seed, ParamType.DryWoodCost, _options.GetDryWoodCostRange());
            
            int maxStage = lastGrowthStage + 1 + SeedUtils.GetRandom(seed, ParamType.MaxStage, _options.GetMaxStageRange(treeType));
            
            int lastFruitStage = 0;
            
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
                StageTime = stageTime,
                MaxStage = maxStage,
                WoodCost = woodCost,
                WoodCostDry = dryWoodCost,
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
            var config = new TreeDataConfig
            {
                RootSeed = data.DataConfig.RootSeed,
                Seed = childSeed,
                Indexes = Indexes,
                TimerStart = _player.Time,
                TreeType = data.DataConfig.TreeType,
                LastGrowthStage = data.DataConfig.LastGrowthStage,
                GrownSpriteIndex = data.DataConfig.GrownSpriteIndex,
                WoodCostDry = data.DataConfig.WoodCostDry,
                LastFruitStage = data.DataConfig.LastFruitStage,
                
                StageTime = GetRandomMutation(childSeed, data.DataConfig.TreeType, ParamType.StageTime, data.DataConfig.StageTime),
                MaxStage = GetRandomMutation(childSeed, data.DataConfig.TreeType, ParamType.MaxStage, data.DataConfig.MaxStage),
                WoodCost = GetRandomMutation(childSeed, data.DataConfig.TreeType, ParamType.WoodCost, data.DataConfig.WoodCost),
            };
            
            return new TreeData(config);
        }
        public TreeData Create(FruitData data)
        {
            throw  new NotImplementedException();
        }

        private int GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, int origin) => 
            Mathf.FloorToInt(GetRandomMutation(childSeed, treeType, paramType, (float)origin));
        private float GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, float origin)
        {
            int mutationPercent = SeedUtils.GetRandom(childSeed, paramType, _options.GetAutoBreedMutationPercentRange(treeType));
    
            float multiplier = 100f + mutationPercent;
            return origin * (multiplier / 100f);
        }
    }
}