using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class TreeFactory : IFactory
    {
        private readonly TreeGenerationOptions _options;
        private readonly Player _player;
        
        private readonly GenomeFactory _genomeFactory;

        private readonly int _seed;
        private int _seedUsages;
        
        public TreeFactory(
            int globalSeed,
            TreeGenerationOptions options,
            GenomeFactory genomeFactory,
            Player player)
        {
            _options = options;
            _genomeFactory = genomeFactory;
            _player = player;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.TreeFactory);
            _seedUsages = 0;
        }

        public List<TreeData> Create(TreeData treeData)
        {
            var config = treeData.TreeGenome;
            
            int newTrees = SeedUtils.GetRandom(config.Seed, ParamType.AutoBreedCount, _options.GetAutoBreedCountRange(config.TreeType));
            int count = treeData.BreedCount + newTrees;
            
            List<TreeData> newTreeData = new List<TreeData>();
            
            for (int i = treeData.BreedCount; i < count; i++)
            {
                newTreeData.Add(Create(config, i));
            }
            treeData.AddBreed(newTrees);

            return newTreeData;
        }

        public TreeData Create() => Create(SeedUtils.GetNewSeed(_seed, _seedUsages++));
        public TreeData Create(int seed) => Create(_genomeFactory.Create(seed));
        public TreeData Create(FruitData data) => Create(data.TreeGenome);
        private TreeData Create(TreeGenomeConfig data, int childIndex) => Create(_genomeFactory.Mutate(data, childIndex));

        private TreeData Create(TreeGenomeConfig genome)
        {
            var config = new TreeDataConfig
            {
                TreeGenomeConfig = genome,
                TimerStart = _player.Time,
            };
            
            return new TreeData(config);
        }

       
        
    }
}