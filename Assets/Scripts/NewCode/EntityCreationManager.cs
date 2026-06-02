using System.Collections.Generic;
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
        
        public List<IEntityData> _entities { get; }
        
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
            
            _entities = new List<IEntityData>();
            
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

        private void OnCreateEntity(EntityCreationRequestSignal signal)
        {
            if (signal.EntityData == null)
                    signal.EntityData = signal.Seed == null ?
                            _factories[signal.EntityType].Create() :
                            _factories[signal.EntityType].Create((int)signal.Seed);
            
            var entityView = Object.Instantiate(_entityBundleLookup[signal.EntityType].EntityView);
            
            _context.SpecialPalette = _entityBundleLookup[signal.EntityType].Palette;
            entityView.Init(signal.EntityData, _context, signal.Position);
            _operationManager.RegisterEntity(entityView);
            _entities.Add(signal.EntityData);
            signal.EntityData.DestroyRequest += OnEntityDestroyRequest;
            signal.EntityData.Start();
        }

        public void Update(float deltaTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(_player.Time);
            }
        }
        
        private void OnEntityDestroyRequest(IEntityData data)
        {
            data.DestroyRequest -= OnEntityDestroyRequest;
            _entities.Remove(data);
        }
    }
}