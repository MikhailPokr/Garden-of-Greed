using System;
using UnityEngine;

namespace Garden
{
    public class GrassFactory : IFactory
    {
        private readonly GrassGenerationOptions _options;
        private readonly GrassPalette _palette;
        
        private readonly int _seed;
        private int _seedUsages;

        public GrassFactory(int globalSeed, GrassGenerationOptions options, GrassPalette palette)
        {
            _options = options;
            _palette = palette;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.GrassFactory);
            _seedUsages = 0;
        }
        
        public int GetNextSeed() => SeedUtils.GetNewSeed(_seed, _seedUsages);

        public GrassData Create(Vector2Int position)
        {
            var newSeed = GetNextSeed();
            _seedUsages++;
            
            var subPosition = SeedUtils.GetRandom(newSeed, ParamType.SubCell, 6);

            GrassDataConfig dataConfig = new GrassDataConfig()
            {
                Seed = newSeed,
                GrowTime = SeedUtils.GetRandom(newSeed, ParamType.GrowTime, _options.GrowTimeRange()),
                MaxStage = _palette.GrassSprites.Count
            };

            return new GrassData(dataConfig, position, subPosition);
        }
    }
}