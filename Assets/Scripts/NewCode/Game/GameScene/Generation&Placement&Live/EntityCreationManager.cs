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
        private VisualContext _context;
        
        private List<IEntityData> _updatableEntities;
        
        public EntityCreationManager(
            VisualContext visualContext,
            List<EntityBundle> entityBundles,
            OperationManager operationManager,
            Player player)
        {
            _operationManager = operationManager;
            _player = player;
            _entityBundleLookup = new Dictionary<EntityType, EntityBundle>();
            foreach (var entityBundle in entityBundles)
                _entityBundleLookup[entityBundle.EntityType] = entityBundle;
            _context = visualContext;
            
            _updatableEntities = new List<IEntityData>();

            SignalBus<EntityCreationRequestSignal>.OnEvent += OnCreateEntity;
        }

        public void Update()
        {
            for (var i = 0; i < _updatableEntities.Count; i++)
            {
                _updatableEntities[i].Update(_player.Time);
            }
        }

        private void OnCreateEntity(EntityCreationRequestSignal signal)
        {
            var entityView = Object.Instantiate(_entityBundleLookup[signal.EntityType].EntityView);
            
            _context.SpecialPalette = _entityBundleLookup[signal.EntityType].Palette;
            
            entityView.Init(signal.EntityData, _context);
            
            _updatableEntities.Add(entityView.EntityData);
            entityView.EntityData.DestroyRequest += OnEntityDestroyRequest;
            _context.SpatialMap.OccupyTile(signal.IntPosition, signal.EntityType);
            _operationManager.RegisterEntity(entityView);
            
            signal.EntityData.Start();
            
            entityView.gameObject.name = signal.EntityType + $" ({signal.Position.x}:{signal.Position.y})";
            
        }
        
        private void OnEntityDestroyRequest(IEntityData data)
        {
            data.DestroyRequest -= OnEntityDestroyRequest;
            _context.SpatialMap.FreeTile(data.Position.Value);
            _updatableEntities.Remove(data);
        }
    }
}