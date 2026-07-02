using System.Linq;
using UnityEngine;

namespace Garden
{
    public class GrassGenerator
    {
        private readonly GrassGenerationOptions _options;
        private readonly GrassPalette _palette;
        private readonly ISpatialMap _spatialMap;

        private readonly int _seed;
        private float _timer;
        private int _useCount;

        public GrassGenerator(int generalSeed, EntityBundle grassBundle, SpatialMap spatialMap)
        {
            _seed = SeedUtils.GetNewSeed(generalSeed, SeedUserType.GrassGenerator);
            _options = grassBundle.GenerationOptions as GrassGenerationOptions;
            _palette = grassBundle.Palette as GrassPalette;
            _spatialMap = spatialMap;
        }

        public void Update(float currentTime)
        {
            if (currentTime < _timer)
                return;
            _timer += SeedUtils.GetRandom(_seed, _useCount, _options.GenerationTimeRange());
            var count = SeedUtils.GetRandom(_seed, _useCount, _options.CountPerUseRange());
            
            var bounds = _spatialMap.Bounds;
            Vector2Int[] positions = new Vector2Int[count];
            for (int i = 0; i < count; i++)
            {
                positions[i] = new Vector2Int(
                    SeedUtils.GetRandom(_seed, _useCount + i, bounds.boundsX), 
                    SeedUtils.GetRandom(_seed, _useCount + i*2, bounds.boundsY));
            }
            
            _useCount += count;
            
            SignalBus<GrassGrowingSignal>.Fire(new GrassGrowingSignal(positions.ToList()));
        }
    }
}