using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class GrassCreationController : IEntityCreationController
    {
        private readonly GrassGenerationOptions _options;
        private readonly GrassPalette _palette;
        
        private readonly GrassFactory _grassFactory;

        public GrassCreationController(int globalSeed, GrassGenerationOptions options, GrassPalette palette)
        {
            _options = options;
            _palette = palette;

            _grassFactory = new GrassFactory(globalSeed, options);
            
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