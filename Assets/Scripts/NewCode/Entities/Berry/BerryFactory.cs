using System;
using UnityEngine;

namespace Garden
{
    public class BerryFactory
    {
        private GeneralPalette _generalPalette;
        private BerryGenerationOptions _options;

        public BerryFactory(GeneralPalette generalPalette, BerryGenerationOptions options)
        {
            _generalPalette = generalPalette;
            _options = options;
        }
        
        public BerryData Create()
        {
            throw new NotImplementedException();
        }
    }
}