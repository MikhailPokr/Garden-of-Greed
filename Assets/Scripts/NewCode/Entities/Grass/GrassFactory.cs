using System;

namespace Garden
{
    public class GrassFactory
    {
        private GeneralPalette _generalPalette;
        private GrassGenerationOptions _options;

        public GrassFactory(GeneralPalette generalPalette, GrassGenerationOptions options)
        {
            _generalPalette = generalPalette;
            _options = options;
        }

        public GrassData Create()
        {
            throw new NotImplementedException();
        }
    }
}