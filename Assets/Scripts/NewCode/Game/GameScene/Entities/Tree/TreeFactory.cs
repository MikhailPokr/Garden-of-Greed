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

        public List<TreeData> Create(TreeData treeData, List<Vector2Int> posRaw)
        {
            var config = treeData.TreeGenome;
            
            int newTrees = SeedUtils.GetRandom(config.Seed, ParamType.AutoBreedCount, _options.GetAutoBreedCountRange(config.TreeType));
            int count = treeData.BreedCount + newTrees;
            
            List<TreeData> newTreeData = new List<TreeData>();
            
            for (int i = treeData.BreedCount; i < count; i++)
            {
                if (posRaw.Count == 0)
                    break;
                int seed = _genomeFactory.GetSeed(treeData.TreeGenome.Seed, i);
                Vector2Int place = posRaw[SeedUtils.GetRandom(seed, ParamType.AutoBreedLocation, posRaw.Count)];
                newTreeData.Add(Create(config, i, posRaw[i]));
                posRaw.Remove(place);
            }
            treeData.AddBreed(newTrees);

            return newTreeData;
        }

        public TreeData Create(Vector2Int pos) => Create(SeedUtils.GetNewSeed(_seed, _seedUsages++), pos);
        public TreeData Create(int seed, Vector2Int pos) => Create(_genomeFactory.Create(seed), pos);
        public TreeData Create(FruitData data, Vector2Int pos) => Create(data.TreeGenome, pos);
        private TreeData Create(TreeGenomeConfig data, int childIndex, Vector2Int pos) => Create(_genomeFactory.Mutate(data, childIndex), pos);

        private TreeData Create(TreeGenomeConfig genome, Vector2Int pos)
        {
            var config = new TreeDataConfig
            {
                TreeGenomeConfig = genome,
                TimerStart = _player.Time,
            };
            
            return new TreeData(config, pos);
        }
    }
}