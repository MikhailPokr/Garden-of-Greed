using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class TreeCreationController : IEntityCreationController
    {
        private ISpatialMap _spatialMap;
        private TreeGenerationOptions _options;
        private TreeFactory _factory;
        private GeoMap _geoMap;
        
        public TreeCreationController(
            int seed,
            EntityBundle bundle,
            ISpatialMap spatialMap,
            GenomeFactory genomeFactory,
            GeoMap geoMap,
            Player player)
        {
            _options = bundle.GenerationOptions as TreeGenerationOptions;
            _spatialMap = spatialMap;
            _geoMap = geoMap;
            _factory = new TreeFactory(seed, _options, genomeFactory, _geoMap, player);

            SignalBus<ArmPlantTreeSignal>.OnEvent += (signal) => CreateViaSeed(signal.Seed, signal.Position);
            SignalBus<ArmPlantFruitSignal>.OnEvent += (signal) => CreateViaFruit(signal.FruitData, signal.Position);
            SignalBus<AutoBreedSignal>.OnEvent += (signal) => OnBreedRequest(signal.TreeData);
        }

        private void CreateViaSeed(int seed, Vector2Int position)
        {
            TreeData treeData = _factory.Create(seed, position);
            
            SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(
                treeData));
        }

        private void CreateViaFruit(FruitData fruitData, Vector2Int position)
        {
            var tree = _factory.Create(fruitData, position);
            if (fruitData.DataConfig.IsGrowth)
            {
                SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(
                    tree));
            }
            else
            {
                DeadShootData deadShoot = new DeadShootData(tree.DataConfig, position);
                SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(
                    deadShoot));
            }
        }
        
        private void OnBreedRequest(TreeData treeData)
        {
            List<Vector2Int> placesRaw = _spatialMap.GetNeighbors(treeData.Position)
                .Where(_spatialMap.IsTileFreeAndValid)
                .ToList();

            List<TreeData> newTreeData = _factory.Create(treeData, placesRaw);
            
            foreach (var tree in newTreeData)
            {
                SignalBus<EntityCreationSignal>.Fire(new EntityCreationSignal(
                    tree));
            }
            
        }
    }
}