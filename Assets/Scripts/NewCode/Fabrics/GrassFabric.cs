using System;

namespace Garden
{
    public class GrassFabric
    {
        private Palette _palette;
        private GrassGenerationOptions _options;

        public GrassFabric(Palette palette, GrassGenerationOptions options)
        {
            _palette = palette;
            _options = options;
        }

        public GrassData Create()
        {
            throw new NotImplementedException();
        }
    }
}