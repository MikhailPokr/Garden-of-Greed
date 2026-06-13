using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Garden
{
    public class FruitFactory : IFactory
    {
        private readonly IFruitPalette _fruitPalette;
        private readonly FruitGenerationOptions _options;
        private readonly Player _player;
        
        private readonly MutationFactory _mutationFactory;
        
        private readonly int _seed;
        private int _seedUsages;
        
        public FruitFactory(
            int globalSeed,
            IFruitPalette palette,
            FruitGenerationOptions options,
            MutationFactory mutationFactory,
            Player player)
        {
            _fruitPalette = palette;
            _options = options;
            _mutationFactory = mutationFactory;
            _player = player;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.FruitFactory);
            _seedUsages = 0;
        }
        public List<FruitData> Create(TreeData treeData)
        {
            var config = treeData.TreeGenome;

            int count = treeData.FruitCount + SeedUtils.GetRandom(config.Seed, ParamType.FruitCount,
                _options.GetCountPerStageRange(config.TreeType));
            
            List<FruitData> newFruitData = new List<FruitData>();

            for (int i = treeData.FruitCount; i < count; i++)
            {
                newFruitData.Add(Create(treeData, i));
            }
            treeData.AddFruit(count);
            
            return newFruitData;
        }
        
        public FruitData Create() => Create(SeedUtils.GetNewSeed(_seed, _seedUsages++));

        public FruitData Create(int seed)
        {
            var config = new FruitDataConfig();
            
            return new FruitData(null, config);
        }

        private FruitData Create(TreeData treeData, int childIndex)
        {
            var config = new FruitDataConfig();
            
            return new FruitData(treeData, config);
        }
    }
}