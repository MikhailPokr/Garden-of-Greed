using System;
using System.Collections.Generic;

namespace Garden
{
    public class TreeDataFactory
    {
        private readonly ITreePalette _treePalette;
        private readonly TreeGenerationOptions _options;
        private readonly Player _player;
        
        private readonly MutationFactory _mutationFactory;

        private readonly int _seed;
        private int _seedUsages;
        
        public TreeDataFactory(
            int globalSeed,
            ITreePalette palette,
            TreeGenerationOptions options,
            MutationFactory mutationFactory,
            Player player)
        {
            _treePalette = palette;
            _options = options;
            _mutationFactory = mutationFactory;
            _player = player;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.TreeFactory);
            _seedUsages = 0;
        }

        public List<TreeData> Create(TreeGenomeConfig treeGenomeConfig)
        {
            int count = SeedUtils.GetRandom(treeGenomeConfig.Seed, ParamType.AutoBreedCount,
                _options.GetAutoBreedCountRange(treeGenomeConfig.TreeType));
            
            List<TreeData> newTreeData = new List<TreeData>();
            
            for (int i = 0; i < count; i++)
            {
                newTreeData.Add(Create(treeGenomeConfig, i));
            }

            return newTreeData;
        }

        public TreeData Create() => Create(SeedUtils.GetNewSeed(_seed, _seedUsages++));
        
        public TreeData Create(int seed)
        {
            TreeType treeType = GenerateType(seed);
            
            var lastGrowthStage = SeedUtils.GetRandom(seed, ParamType.LastGrowthStage, _options.GetGrowthLastStageRange(treeType));
            lastGrowthStage = Math.Clamp(lastGrowthStage, 0, _treePalette.StageSpritesCount);
            var sprite = treeType switch
            {
                _ when treeType.HasFlag(TreeType.Evil) => SeedUtils.GetRandom(seed, ParamType.TreeSprite,
                    _treePalette.TreeEvilSpritesCount),
                _ => SeedUtils.GetRandom(seed, ParamType.TreeSprite, _treePalette.TreeSpritesCount)
            };
            var greenOffset = SeedUtils.GetRandom(seed, ParamType.GreenColorOffset, _options.GetGreenOffsetRange());
            
            var stageTime = SeedUtils.GetRandom(seed, ParamType.StageTime, _options.GetStageTimeRange(treeType));
            var woodCost = SeedUtils.GetRandom(seed, ParamType.WoodCost, _options.GetWoodCostRange(treeType));
            var dryWoodCost = SeedUtils.GetRandom(seed, ParamType.DryWoodCost, _options.GetDryWoodCostRange());
            var woodColor = SeedUtils.GetRandom(seed, ParamType.WoodColor, _treePalette.WoodColorsCount);
            
            var maxStage = lastGrowthStage + 1 + SeedUtils.GetRandom(seed, ParamType.MaxStage, _options.GetMaxStageRange(treeType));
            
            var lastFruitStage = 0;
            
            if (treeType.HasFlag(TreeType.Fruit))
            {
                lastFruitStage = SeedUtils.GetRandom(seed, ParamType.LastFruitStage, _options.GetLastFruitStageRange(treeType));
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
                MaxStage = maxStage,
                WoodCost = woodCost,
                WoodCostDry = dryWoodCost,
            };

            var config = new TreeDataConfig()
            {
                TreeGenomeConfig = genome,
                TimerStart = _player.Time,
            };
            
            return new TreeData(config);
        }
        
        public TreeData Create(FruitData data)
        {
            var config = new TreeDataConfig
            {
                TreeGenomeConfig = _mutationFactory.Create(data),
                TimerStart = _player.Time,
            };
            
            return new TreeData(config);
        }

        private TreeData Create(TreeGenomeConfig data, int childIndex)
        {
            var config = new TreeDataConfig
            {
                TreeGenomeConfig = _mutationFactory.Create(data, childIndex),
                TimerStart = _player.Time,
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
        
        
    }
}