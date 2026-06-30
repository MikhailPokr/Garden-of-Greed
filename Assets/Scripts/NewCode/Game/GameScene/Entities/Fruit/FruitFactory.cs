using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Garden
{
    public class FruitFactory : IFactory
    {
        private readonly IFruitPalette _fruitPalette;
        private readonly FruitGenerationOptions _options;
        private readonly Player _player;
        
        private readonly GenomeFactory _genomeFactory;
        
        public FruitFactory(
            IFruitPalette palette,
            FruitGenerationOptions options,
            GenomeFactory genomeFactory,
            Player player)
        {
            _fruitPalette = palette;
            _options = options;
            _genomeFactory = genomeFactory;
            _player = player;
        }
        public List<FruitData> Create(TreeData treeData)
        {
            var config = treeData.TreeGenome;
            
            int newFruits = SeedUtils.GetRandom(config.Seed, ParamType.FruitCount, _options.GetCountPerStageRange(config.TreeType));
            int count = treeData.FruitCount + newFruits;
            
            List<FruitData> newFruitData = new List<FruitData>();

            for (int i = treeData.FruitCount; i < count; i++)
            {
                newFruitData.Add(Create(treeData, i));
            }
            treeData.AddFruit(newFruits);
            
            return newFruitData;
        }

        private FruitData Create(TreeData treeData, int childIndex)
        {
            var genome = _genomeFactory.MutateWithQuality(treeData.TreeGenome, childIndex);
            
            var isGrowth = SeedUtils.GetRandom(genome.Seed, ParamType.GrowthChance, new Vector2(0, 1)) < _options.GetGrowUpChance(genome.TreeType);
            var colorOffset =
                SeedUtils.GetRandom(genome.Seed, ParamType.FruitColorOffset, _options.GetColorOffsetRange());
            
            var config = new FruitDataConfig()
            {
                TreeGenome = genome,
                IsGrowth = isGrowth,
                ColorOffset = colorOffset,
                TimerStart = _player.Time,
            };
            
            return new FruitData(treeData, config);
        }
    }
}