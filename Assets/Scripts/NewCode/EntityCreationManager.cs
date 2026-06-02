using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class EntityCreationManager
    {
        private readonly int _seed;
        private readonly List<EntityBundle> _entityBundles;
        private readonly GeneralPalette _generalPalette;
        private readonly SpriteOrderOptions _spriteOrderOptions;
        private readonly OperationManager _operationManager;
        private readonly Player _player;
        private readonly Field _field;

        private Dictionary<EntityType, EntityBundle> _entityBundleLookup;
        private Dictionary<EntityType, IFactory> _factories;
        
        public List<IEntityData> _entities { get; }
        
        public EntityCreationManager(int seed, List<EntityBundle> entityBundles, SpriteOrderOptions spriteOrderOptions, OperationManager operationManager, Player player, Field field)
        {
            _seed = seed;
            _entityBundles = entityBundles;
            _spriteOrderOptions = spriteOrderOptions;
            _operationManager = operationManager;
            _player = player;
            _field = field;
            
            _entities = new List<IEntityData>();
            
            _entityBundleLookup = new Dictionary<EntityType, EntityBundle>();
            
            foreach (var entityBundle in _entityBundles)
                _entityBundleLookup[entityBundle.EntityType] = entityBundle;
            
            _factories = new Dictionary<EntityType, IFactory>()
            {
                {
                    EntityType.Tree,
                    new TreeFactory(_seed, _entityBundleLookup[EntityType.Tree], _player)
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
            
            if (signal.EntityData == null)
                signal.EntityData = _factories[signal.EntityType].Create();
            
            entityView.Init(signal.EntityData, _spriteOrderOptions, _entityBundleLookup[signal.EntityType].Palette, _field, signal.Position);
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