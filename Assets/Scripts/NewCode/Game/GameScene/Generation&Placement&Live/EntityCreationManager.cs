using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class EntityCreationManager
    {
        private readonly ToolManager _toolManager;
        private readonly Player _player;
        
        private readonly Dictionary<EntityType, EntityBundle> _entityBundleLookup;
        private VisualContext _context;
        
        private List<IEntityData> _updatableEntities;
        private Dictionary<IEntityData, EntityView> _createdViews;
        
        public EntityCreationManager(
            VisualContext visualContext,
            List<EntityBundle> entityBundles,
            ToolManager toolManager,
            Player player)
        {
            _toolManager = toolManager;
            _player = player;
            _entityBundleLookup = new Dictionary<EntityType, EntityBundle>();
            foreach (var entityBundle in entityBundles)
                _entityBundleLookup[entityBundle.EntityType] = entityBundle;
            _context = visualContext;
            
            _updatableEntities = new List<IEntityData>();
            _createdViews = new Dictionary<IEntityData, EntityView>();

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
            var entityView = Object.Instantiate(_entityBundleLookup[signal.EntityData.EntityType].EntityView);
            
            _context.SpecialPalette = _entityBundleLookup[signal.EntityData.EntityType].Palette;
            
            entityView.Init(signal.EntityData, _context);
            
            _updatableEntities.Add(entityView.EntityData);
            _createdViews[signal.EntityData] = entityView;
            entityView.EntityData.DestroyRequest += OnEntityDestroyRequest;
            _context.SpatialMap.OccupyTile(signal.Position, signal.EntityData.EntityType);
            
            signal.EntityData.Start();
            
            entityView.gameObject.name = signal.EntityData.EntityType + $" ({signal.Position.x}:{signal.Position.y})";

            if (signal.EntityData is IDependentEntity entity)
            {
                var host = _createdViews[entity.HostEntity];
                host.SetEntity(entityView);
            }
        }
        
        private void OnEntityDestroyRequest(IEntityData data)
        {
            data.DestroyRequest -= OnEntityDestroyRequest;
            _context.SpatialMap.FreeTile(data.Position.Value, data.EntityType);
            _updatableEntities.Remove(data);
            _createdViews.Remove(data);
        }
    }
}