using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class GrassCreationController : IEntityCreationController
    {
        private readonly GrassGenerationOptions _options;
        private readonly GrassPalette _palette;
        
        private readonly GrassFactory _grassFactory;

        public GrassCreationController(int globalSeed, EntityBundle bundle)
        {
            _options = bundle.GenerationOptions as GrassGenerationOptions;
            _palette = bundle.Palette as GrassPalette;

            _grassFactory = new GrassFactory(globalSeed, _options, _palette);
            
            SignalBus<GrassGrowingSignal>.OnEvent += OnGrassGrowing;
        }

        private void OnGrassGrowing(GrassGrowingSignal signal)
        {
            foreach (var position in signal.Positions)
            {
                var grass = _grassFactory.Create(position);
                SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(grass));
            }
        }
    }
}