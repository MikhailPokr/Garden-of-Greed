using System;
using UnityEngine;

namespace Garden
{
    public class BerryFabric
    {
        private Palette _palette;
        private BerryGenerationOptions _options;

        public BerryFabric(Palette palette, BerryGenerationOptions options)
        {
            _palette = palette;
            _options = options;
        }
        
        public BerryData Create()
        {
            throw new NotImplementedException();
        }
    }
}