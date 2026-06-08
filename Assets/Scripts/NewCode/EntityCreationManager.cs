using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class EntityCreationManager
    {
        private readonly OperationManager _operationManager;
        private readonly Player _player;

        private readonly Dictionary<EntityType, EntityBundle> _entityBundleLookup;
        private readonly Dictionary<EntityType, IFactory> _factories;
        
        private VisualContext _context;
        
        public Dictionary<Vector2Int, List<EntityView>> Entities { get; }
        private List<EntityView> _updatableEntities;
        
        public EntityCreationManager(
            int seed,
            VisualContext visualContext,
            List<EntityBundle> entityBundles,
            OperationManager operationManager,
            Player player)
        {
            _operationManager = operationManager;
            _player = player;
            _context = visualContext;

            Entities = new Dictionary<Vector2Int, List<EntityView>>();
            _updatableEntities = new List<EntityView>();
            
            _entityBundleLookup = new Dictionary<EntityType, EntityBundle>();
            
            foreach (var entityBundle in entityBundles)
                _entityBundleLookup[entityBundle.EntityType] = entityBundle;
            
            _factories = new Dictionary<EntityType, IFactory>()
            {
                {
                    EntityType.Tree,
                    new TreeFactory(seed, _entityBundleLookup[EntityType.Tree], _player)
                },
            };

            SignalBus<EntityCreationRequestSignal>.OnEvent += OnCreateEntity;
        }

        public bool CheckPlace<T>(Vector2Int position) where T : EntityView => 
            !Entities.ContainsKey(position) ||
            !Entities[position].Exists(x => x is T);

        private void OnCreateEntity(EntityCreationRequestSignal signal)
        {
            Debug.Log($"Creating entity {signal.EntityType.ToString()} with position {signal.Position}");

            signal.EntityData = _factories[signal.EntityType].Create(signal);
            
            var entityView = Object.Instantiate(_entityBundleLookup[signal.EntityType].EntityView);
            
            _context.SpecialPalette = _entityBundleLookup[signal.EntityType].Palette;
            
            signal.EntityData.SetPosition(signal.Position);
            entityView.Init(signal.EntityData, _context);
            _operationManager.RegisterEntity(entityView);
            
            if (!Entities.ContainsKey(signal.Position))
                Entities.Add(signal.Position, new List<EntityView>());
            Entities[signal.Position].Add(entityView);
            _updatableEntities.Add(entityView);
            
            signal.EntityData.DestroyRequest += OnEntityDestroyRequest;
            signal.EntityData.Start();
            
            entityView.gameObject.name = signal.EntityType.ToString() + $" ({signal.Position.x}:{signal.Position.y})";

            if (signal.EntityType == EntityType.Tree)
            {
                var Tdata = (TreeData)signal.EntityData;
                Tdata.BreedRequest += OnBreedRequest;
                Tdata.FruitRequest += OnFruitRequest;
            }
        }

        private void OnFruitRequest(TreeData treeData)
        {
            List<FruitData> fruits = _factories[EntityType.Fruit]
                .Create(Entities[(Vector2Int)treeData.Position].Find(x => x.EntityData == treeData))
                .OfType<FruitData>()
                .ToList();

            foreach (var fruit in fruits)
            {
                
            }
        }

        private void OnBreedRequest(TreeData treeData)
        {
            List<Vector2Int> placesRaw = _context.Field.GetNeighbors((Vector2Int)treeData.Position)
                .Where(CheckPlace<TreeView>)
                .ToList();
            
            List<TreeData> newTreeData = _factories[EntityType.Tree]
                .Create(Entities[(Vector2Int)treeData.Position].Find(x => x.EntityData == treeData))
                .OfType<TreeData>()
                .ToList();
            
            foreach (var tree in newTreeData)
            {
                if (placesRaw.Count == 0)
                    break;
                Vector2Int place = placesRaw[SeedUtils.GetRandom(tree.DataConfig.Seed, ParamType.AutoBreedLocation, placesRaw.Count)];
                placesRaw.Remove(place);
                OnCreateEntity(new EntityCreationRequestSignal(EntityType.Tree, tree, place));
            }
            
        }

        public void Update(float deltaTime)
        {
            for (var i = 0; i < _updatableEntities.Count; i++)
            {
                _updatableEntities[i].EntityData.Update(_player.Time);
            }
        }
        
        private void OnEntityDestroyRequest(IEntityData data)
        {
            if (data.Position == null)
                throw new System.Exception("Entity without position deleted");
            data.DestroyRequest -= OnEntityDestroyRequest;
            Entities[(Vector2Int)data.Position].RemoveAll(x => x.EntityData == data);
            _updatableEntities.RemoveAll(x => x.EntityData == data);
        }
    }
}