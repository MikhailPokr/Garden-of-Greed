using System;
using UnityEngine;

namespace Garden
{
    public class BerryFactory
    {
        private readonly BerryPalette _berryPalette;
        private readonly BerryGenerationOptions _options;
        private readonly ISpatialMap _spatialMap;
        
        private readonly int _seed;
        private int _seedUsages;

        public BerryFactory(int globalSeed, BerryGenerationOptions options, BerryPalette berryPalette, ISpatialMap spatialMap)
        {
            _berryPalette = berryPalette;
            _options = options;
            _spatialMap = spatialMap;
            
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.GrassFactory);
            _seedUsages = 0;
        }
        public int GetNextSeed() => SeedUtils.GetNewSeed(_seed, _seedUsages);
        public BerryData Create(Vector2Int position)
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
                    cell = new CellData(CellType.Sub, EntityType.Berry, position, subPosition + delta);
                    if (_spatialMap.IsTileFreeAndValid(cell))
                    {
                        BerryDataConfig dataConfig = new BerryDataConfig()
                        {
                            Seed = newSeed,
                            Cost = SeedUtils.GetRandom(newSeed, ParamType.BerryCost, _options.GetCostRange()),
                            Regeneration = SeedUtils.GetRandom(newSeed, ParamType.BerryRegeneration, _options.GetRegenerationValueRange()),
                            ColorIndex = SeedUtils.GetRandom(newSeed, ParamType.BerryColor, _berryPalette.BerryColors.Count) //TODO: change to interface 
                        };

                        return new BerryData(dataConfig, position, cell.SubCell);
                    }
                }
            }

            return null;
        }
    }
}