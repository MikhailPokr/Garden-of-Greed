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
        
        public TreeFactory(TreePalette palette, TreeGenerationOptions options, Player player)
        {
            _treePalette = palette;
            _options = options;
            _player = player;
        }
        
        public TreeData Create()
        {
            var seed = Random.Range(int.MinValue, int.MaxValue);

            TreeType treeType = GenerateType(seed);
            
            var lastGrowthStage = IFactory.GetRandom(seed, ParamType.LastGrowthStage, _options.GetGrowthLastStageRange(treeType));
            lastGrowthStage = Math.Clamp(lastGrowthStage, 0, _treePalette.StageSprites.Count);
            var sprite = treeType switch
            {
                _ when treeType.HasFlag(TreeType.Evil) => IFactory.GetRandom(seed, ParamType.TreeSprite,
                    _treePalette.TreeEvilSprites.Count),
                _ => IFactory.GetRandom(seed, ParamType.TreeSprite, _treePalette.TreeSprites.Count)
            };
            
            var stageTime = IFactory.GetRandom(seed, ParamType.StageTime, _options.GetStageTimeRange(treeType));
            var woodCost = IFactory.GetRandom(seed, ParamType.WoodCost, _options.GetWoodCostRange(treeType));
            var dryWoodCost = IFactory.GetRandom(seed, ParamType.DryWoodCost, _options.GetDryWoodCostRange());
            
            int maxStage = lastGrowthStage + 1 + IFactory.GetRandom(seed, ParamType.MaxStage, _options.GetMaxStageRange(treeType));
            
            int lastFruitStage = 0;
            
            if (treeType.HasFlag(TreeType.Fruit))
            {
                lastFruitStage = IFactory.GetRandom(seed, ParamType.LastFruitStage, _options.GetLastFruitStageRange(treeType));
            }
            
            var config = new TreeDataConfig
            {
                Seed = seed,
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
                if (IFactory.GetRandom(seed, treeTypeConfig.ParamType, 100) < treeTypeConfig.ChanceInPercent)
                    treeType |= treeTypeConfig.TreeType;
            }
            return treeType;
        }

        public TreeData Create(TreeData data, int childIndex)
        {
            int childSeed = IFactory.GetRandom(
                data.DataConfig.Seed, 
                (ParamType)((int)ParamType.ChildSeedOffset + childIndex), 
                int.MaxValue
            );
            var config = new TreeDataConfig
            {
                Seed = childSeed,
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
            Mathf.FloorToInt(GetRandomMutation(childSeed, treeType, paramType, origin));
        private float GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, float origin)
        {
            int mutationPercent = IFactory.GetRandom(childSeed, paramType, _options.GetAutoBreedMutationPercentRange(treeType));
    
            float multiplier = 100f + mutationPercent;
            return origin * (multiplier / 100f);
        }
    }
}