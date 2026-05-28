using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Garden
{
    public class TreeFabric
    {
        private readonly Palette _palette;
        private readonly TreeGenerationOptions _options;
        private readonly Player _player;
        
        public TreeFabric(Palette palette, TreeGenerationOptions options, Player player)
        {
            _palette = palette;
            _options = options;
            _player = player;
        }
        
        public TreeData Create()
        {
            TreeDataConfig config;
            
            var stagesSprites = _palette.StageSprites.GetRange(0, Random
                            .Range(1+_options.MaximumStagesReduction, _palette.StageSprites.Count+1));
            int dryWoodCost = Random.Range(_options.DryWoodCostRange.x, _options.DryWoodCostRange.y);
            Sprite dieSprite = _palette.DieSprite;
            
            bool isFruit = Random.value < _options.FruitChance;
            bool isEvil = Random.value < _options.EvilChance;

            if (!isFruit)
            {
                stagesSprites.Add(_palette.TreeSprites[Random.Range(0, _palette.TreeSprites.Count)]);
                config = new TreeDataConfig()
                {
                    AutoBreedingTree = Random.value < _options.AutoBreedChance,
                    BreedCount = Random.Range(_options.AutoBreedCountRange.x, _options.AutoBreedCountRange.y),
                    StageTime = Random.Range(_options.NormalStageTimeRange.x, _options.NormalStageTimeRange.y),
                    MaxStage = Random.Range(stagesSprites.Count + _options.NormalMaxStageRange.x, stagesSprites.Count +
                        _options.NormalMaxStageRange.y),
                    WoodCost = Random.Range(_options.NormalWoodCostRange.x, _options.NormalWoodCostRange.y),
                    TimerStart = _player.Time,
                };
            }
            else if (!isEvil)
            {
                int lastFruitStage = Random.Range(_options.LastFruitStageRange.x, _options.LastFruitStageRange.y);
                stagesSprites.Add(_palette.TreeSprites[Random.Range(0, _palette.TreeSprites.Count)]);
                config = new TreeDataConfig()
                {
                    IsFruitTree = true,
                    StageTime = Random.Range(_options.FruitStageTimeRange.x, _options.FruitStageTimeRange.y),
                    LastFruitStage = lastFruitStage,
                    MaxStage = Random.Range(lastFruitStage + _options.FruitMaxStageRange.x, stagesSprites.Count +
                        _options.FruitMaxStageRange.y),
                    WoodCost = Random.Range(_options.FruitWoodCostRange.x, _options.FruitWoodCostRange.y),
                    BaseFruitCost = Random.Range(_options.BaseFruitCostRange.x, _options.BaseFruitCostRange.y),
                    FruitSprite = _palette.FruitSprites[Random.Range(0, _palette.FruitSprites.Count)],
                    TimerStart = _player.Time,
                };
            }
            else
            {
                stagesSprites.Add(_palette.TreeEvilSprites[Random.Range(0, _palette.TreeSprites.Count)]);
                int lastFruitStage = Random.Range(_options.EvilLastFruitStageRange.x, _options.EvilLastFruitStageRange.y);
                config = new TreeDataConfig()
                {
                    IsFruitTree = true,
                    IsEvilTree =  true,
                    StageTime = Random.Range(_options.EvilStageTimeRange.x, _options.EvilStageTimeRange.y),
                    LastFruitStage = lastFruitStage,
                    MaxStage = Random.Range(lastFruitStage + _options.EvilMaxStageRange.x, stagesSprites.Count +
                        _options.EvilMaxStageRange.y),
                    WoodCost = Random.Range(_options.EvilWoodCostRange.x, _options.EvilWoodCostRange.y),
                    BaseFruitCost = Random.Range(_options.EvilBaseFruitCostRange.x, _options.EvilBaseFruitCostRange.y),
                    FruitSprite = _palette.FruitEvilSprites[Random.Range(0, _palette.FruitEvilSprites.Count)],
                    TimerStart = _player.Time,
                };
            }
            config.StagesSprites = stagesSprites;
            config.DieSprite = dieSprite;
            config.WoodCostDry = dryWoodCost;
            
            return new TreeData(config);
        }
        public TreeData Create(TreeData data)
        {
            var config = new TreeDataConfig()
            {
                AutoBreedingTree = Random.value < _options.AutoBreedChance,
                BreedCount = Random.Range(_options.AutoBreedCountRange.x, _options.AutoBreedCountRange.y),
                StageTime = Mathf.FloorToInt(data.DataConfig.StageTime / 100f * (100 +
                                                Random.Range(_options.AutoBreedMutationRange.x,
                                                    _options.AutoBreedMutationRange.y))),
                MaxStage = data.DataConfig.MaxStage,
                WoodCost = Mathf.FloorToInt(data.DataConfig.WoodCost / 100f * (100 + 
                                                Random.Range(_options.AutoBreedMutationRange.x,
                                                    _options.AutoBreedMutationRange.y))),
                TimerStart = _player.Time,
                WoodCostDry = data.DataConfig.WoodCost,
                StagesSprites = data.DataConfig.StagesSprites,
                DieSprite = data.DataConfig.DieSprite
            };
            
            return new TreeData(config);
        }
        public TreeData Create(FruitData data)
        {
            var config = new TreeDataConfig()
            {
                IsFruitTree = true,
                IsEvilTree =  true,
                StageTime = data.DataConfig.ParentTree.DataConfig.StageTime + data.DataConfig.DeltaStageTime,
                LastFruitStage = data.DataConfig.ParentTree.DataConfig.LastFruitStage,
                MaxStage = data.DataConfig.ParentTree.DataConfig.MaxStage,
                WoodCost = data.DataConfig.ParentTree.DataConfig.WoodCost + data.DataConfig.DeltaWoodCost,
                BaseFruitCost = data.DataConfig.ParentTree.DataConfig.BaseFruitCost + data.DataConfig.DeltaFruitCost,
                TimerStart = _player.Time,
                WoodCostDry = data. DataConfig.ParentTree.DataConfig.WoodCost,
                StagesSprites = data.DataConfig.ParentTree.DataConfig.StagesSprites,
                FruitSprite = data.DataConfig.ParentTree.DataConfig.FruitSprite,
                DieSprite = data.DataConfig.ParentTree.DataConfig.DieSprite
            };
            
            return new TreeData(config);
        }
        
    }
}