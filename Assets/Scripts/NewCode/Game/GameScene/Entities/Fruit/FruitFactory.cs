using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Garden
{
    public class FruitFactory
    {
        private readonly FruitPalette _fruitPalette;
        private readonly FruitGenerationOptions _options;
        private readonly Player _player;
        
        private readonly int _seed;
        private int _seedUsages;
        
        public FruitFactory(int globalSeed, EntityBundle bundle, Player player)
        {
        }
        
        
        public FruitData Create() => Create(SeedUtils.GetNewSeed(_seed, _seedUsages++));

        public FruitData Create(int seed)
        {
            var config = new FruitDataConfig();
            
            return new FruitData(config);
        }
        public List<IEntityData> Create(EntityView origin)
        {
            throw new NotImplementedException();
        }

        public List<IEntityData> Create(TreeGenomeConfig treeGenomeConfig)
        {
            throw new NotImplementedException();
        }

        public FruitData Create(TreeData origin, int childIndex)
        {
            var config = new FruitDataConfig();
            
            return new FruitData(config);
        }
        
        private float GetRandomMutation(int childSeed, TreeType treeType, ParamType paramType, float origin)
        {
            throw new NotImplementedException();
            /*int mutationPercent = SeedUtils.GetRandom(childSeed, paramType, _options);

            float multiplier = 100f + mutationPercent;
            return origin * (multiplier / 100f);*/
        }


        
    }
}