namespace Garden
{
    public class BerryCreationManager : IEntityCreationController
    {
        private readonly BerryGenerationOptions _options;
        private readonly BerryPalette _palette;
        
        private readonly BerryFactory _berryFactory;

        public BerryCreationManager(int globalSeed, EntityBundle bundle, ISpatialMap spatialMap)
        {
            _options = bundle.GenerationOptions as BerryGenerationOptions;
            _palette = bundle.Palette as BerryPalette;

            _berryFactory = new BerryFactory(globalSeed, _options, _palette, spatialMap);
            
            SignalBus<BerryGrowingSignal>.OnEvent += OnBerryGrowing;
        }

        private void OnBerryGrowing(BerryGrowingSignal signal)
        {
            foreach (var position in signal.Positions)
            {
                var berry = _berryFactory.Create(position);
                if (berry != null)
                    SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(berry));
            }
        }
    }
}