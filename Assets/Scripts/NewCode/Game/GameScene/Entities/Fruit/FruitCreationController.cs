using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class FruitCreationController : IEntityCreationController
    {
        private ISpatialMap _spatialMap;
        private FruitGenerationOptions _options;
        private FruitFactory _factory;
        
        public FruitCreationController(
            int seed,
            EntityBundle bundle,
            ISpatialMap spatialMap,
            GenomeFactory genomeFactory,
            Player player)
        {
            _options = bundle.GenerationOptions as FruitGenerationOptions;
            _spatialMap = spatialMap;
            _factory = new FruitFactory(seed, bundle.Palette as IFruitPalette, _options, genomeFactory, player);
            
            SignalBus<FruitProduceSignal>.OnEvent += OnFruitProduce;
        }

        private void OnFruitProduce(FruitProduceSignal signal)
        {
            Vector2Int tPos = signal.TreeData.Position;

            List<FruitData> fruits = _factory.Create(signal.TreeData);
            
            foreach (var fruit in fruits)
            {
                SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(fruit));
            }
        }
    }
}