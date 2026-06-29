using System;
using UnityEngine;

namespace Garden
{
    public class GrassFactory : IFactory
    {
        private GrassGenerationOptions _options;
        
        private readonly int _seed;
        private int _seedUsages;

        public GrassFactory(int globalSeed, GrassGenerationOptions options)
        {
            _options = options;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.GrassFactory);
            _seedUsages = 0;
        }
        
        public int GetNextSeed() => SeedUtils.GetNewSeed(_seed, _seedUsages);

        public GrassData Create(Vector2Int position)
        {
            var newSeed = GetNextSeed();
            _seedUsages++;
            
            var subPosition = SeedUtils.GetRandom(newSeed, ParamType.SubCell, position);
            
            GrassDataConfig dataConfig = new GrassDataConfig();

            return new GrassData(dataConfig, position, subPosition);
        }
    }
}