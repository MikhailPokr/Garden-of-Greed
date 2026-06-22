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
        
        public TreeCreationController(
            int seed,
            EntityBundle bundle,
            ISpatialMap spatialMap,
            GenomeFactory genomeFactory,
            Player player)
        {
            _options = bundle.GenerationOptions as TreeGenerationOptions;
            _spatialMap = spatialMap;
            _factory = new TreeFactory(seed, _options, genomeFactory, player);

            SignalBus<AutoBreedSignal>.OnEvent += (signal) => this.OnBreedRequest(signal.TreeData);
            SignalBus<ArmPlantTreeSignal>.OnEvent += (signal) => CreateViaSeed(signal.Seed, signal.Position);
        }

        private void CreateViaSeed(int seed, Vector2Int position)
        {
            TreeData treeData = _factory.Create(seed);
            treeData.SetPosition(position);
            
            
            SignalBus<EntityCreationRequestSignal>.Fire(new EntityCreationRequestSignal(
                treeData,
                position));
        }

        private void OnBreedRequest(TreeData treeData)
        {
            List<Vector2Int> placesRaw = _spatialMap.GetNeighbors((Vector2Int)treeData.Position)
                .Where(_spatialMap.IsTileFreeAndValid)
                .ToList();

            List<TreeData> newTreeData = _factory.Create(treeData);
            
            foreach (var tree in newTreeData)
            {
                if (placesRaw.Count == 0)
                    break;
                Vector2Int place = placesRaw[SeedUtils.GetRandom(tree.TreeGenome.Seed, ParamType.AutoBreedLocation, placesRaw.Count)];
                placesRaw.Remove(place);
                tree.SetPosition(place);
                SignalBus<EntityCreationRequestSignal>.Fire(new EntityCreationRequestSignal(
                    tree,
                    place));
            }
            
        }
    }
}