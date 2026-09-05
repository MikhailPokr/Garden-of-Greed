using System;
using UnityEngine;

namespace Garden
{
    public class GrassFactory : IFactory
    {
        private readonly GrassGenerationOptions _options;
        private readonly GrassPalette _palette;
        private readonly ISpatialMap _spatialMapmap;
        
        private readonly int _seed;
        private int _seedUsages;

        public GrassFactory(int globalSeed, GrassGenerationOptions options, GrassPalette palette, ISpatialMap spatialMap)
        {
            _options = options;
            _palette = palette;
            _spatialMapmap = spatialMap;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.GrassFactory);
            _seedUsages = 0;
        }
        
        public int GetNextSeed() => SeedUtils.GetNewSeed(_seed, _seedUsages);

        public GrassData Create(Vector2Int position)
        {
            var newSeed = GetNextSeed();
            _seedUsages++;
            
            var subPosition = SeedUtils.GetRandom(newSeed, ParamType.SubCell, 6);
            CellData cell;
            for (int i = 0; i < 6; i++)
            {
                int delta = i;
                for (int j = 0; j < 2; j++)
                {
                    if (j % 2 == 0)
                        delta *= -1;
                    if (subPosition + delta < 0 || subPosition + delta >= 6)
                        continue;
                    cell = new CellData(CellType.Sub, EntityType.Grass, position, subPosition + delta);
                    if (_spatialMapmap.IsTileFreeAndValid(cell, EntityType.Grass))
                    {
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

            return null;
        }
    }
}