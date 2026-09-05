using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class SubCellContentGenerator
    {
        private readonly GrassGenerationOptions _grassOptions;
        private readonly BerryGenerationOptions _berryOptions;
        private readonly ISpatialMap _spatialMap;

        private readonly int _seed;
        private float _timer;
        private int _useCount;

        public SubCellContentGenerator(int generalSeed, EntityBundle grassBundle, EntityBundle berryBundle, SpatialMap spatialMap)
        {
            _seed = SeedUtils.GetNewSeed(generalSeed, SeedUserType.GrassGenerator);
            _grassOptions = grassBundle.GenerationOptions as GrassGenerationOptions;
            _berryOptions = berryBundle.GenerationOptions as BerryGenerationOptions;
            _spatialMap = spatialMap;
        }

        public void Update(float currentTime)
        {
            if (currentTime < _timer)
                return;
            _timer += SeedUtils.GetRandom(_seed, _useCount, _grassOptions.GenerationTimeRange());
            var count = SeedUtils.GetRandom(_seed, _useCount, _grassOptions.CountPerUseRange());
            
            var bounds = _spatialMap.Bounds;
            List<Vector2Int> grassPositions = new List<Vector2Int>();
            List<Vector2Int> berryPositions = new List<Vector2Int>();
            for (int i = 0; i < count; i++)
            {
                var pos = new Vector2Int(
                    SeedUtils.GetRandom(_seed, _useCount + i, bounds.boundsX), 
                    SeedUtils.GetRandom(_seed, _useCount + i*2, bounds.boundsY));
                if (SeedUtils.GetRandom(_seed, ParamType.BerryChance + _useCount + i) < _berryOptions.GetBerryChance())
                    berryPositions.Add(pos);
                else
                    grassPositions.Add(pos);
            }
            
            _useCount += count;
            
            SignalBus<GrassGrowingSignal>.Fire(new GrassGrowingSignal(grassPositions.ToList()));
            SignalBus<BerryGrowingSignal>.Fire(new BerryGrowingSignal(berryPositions.ToList()));
        }
    }
}