using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class EntityCreationManager
    {
        private readonly Dictionary<EntityType, EntityBundle> _entityBundleLookup;
        private VisualContext _context;
        
        private List<IEntityData> _updatableEntities;
        private Dictionary<IEntityData, EntityView> _createdViews;
        private Dictionary<(Vector2Int position, int subPosition), IStackingSubEntity> _stackingSubEntities;
        
        public EntityCreationManager(
            VisualContext visualContext,
            List<EntityBundle> entityBundles)
        {
            _entityBundleLookup = new Dictionary<EntityType, EntityBundle>();
            foreach (var entityBundle in entityBundles)
                _entityBundleLookup[entityBundle.EntityType] = entityBundle;
            _context = visualContext;
            
            _updatableEntities = new List<IEntityData>();
            _createdViews = new Dictionary<IEntityData, EntityView>();
            _stackingSubEntities = new Dictionary<(Vector2Int position, int subPosition), IStackingSubEntity>();

            SignalBus<EntityCreationSignal>.OnEvent += OnCreateEntity;
        }
        
        public void Update(float currentTime)
        {
            for (var i = 0; i < _updatableEntities.Count; i++)
            {
                _updatableEntities[i].Update(currentTime);
            }
        }

        private void OnCreateEntity(EntityCreationSignal signal)
        {
            if (signal.EntityData is IStackingSubEntity stackingSubEntity)
            {
                if (_stackingSubEntities
                    .TryGetValue((stackingSubEntity.Position, stackingSubEntity.SubPosition),
                        out var stackingSubEntityData))
                {
                    stackingSubEntityData.Grow();
                    return;
                }
            }
            
            var entityView = Object.Instantiate(_entityBundleLookup[signal.EntityData.EntityType].EntityView);
            
            _context.SpecialPalette = _entityBundleLookup[signal.EntityData.EntityType].Palette;
            
            entityView.Init(signal.EntityData, _context);
            
            Add(entityView);

            switch (signal.EntityData)
            {
                case ISubEntity subEntity:
                    _context.SpatialMap.OccupySubTile(subEntity.Position, subEntity.SubPosition, signal.EntityData.EntityType);
                    entityView.gameObject.name = signal.EntityData.EntityType + $" ({subEntity.Position.x}:{subEntity.Position.y}|{subEntity.SubPosition})";
                    break;
                case { } entityData:
                    _context.SpatialMap.OccupyTile(entityData.Position, signal.EntityData.EntityType);
                    entityView.gameObject.name = signal.EntityData.EntityType + $" ({entityData.Position.x}:{entityData.Position.y})";
                    break;
            }
            
            entityView.EntityData.CommandRequest += OnEntityDestroyRequest;
            
            signal.EntityData.Start();
            
            if (signal.EntityData is IDependentEntity entity)
            {
                var host = _createdViews[entity.HostEntity];
                host.SetEntity(entityView);
            }
        }
        
        private void Add(EntityView entityView)
        {
            var data = entityView.EntityData;
            
            _updatableEntities.Add(data);
            _createdViews[data] = entityView;
            
            if (data is IStackingSubEntity stackingSubEntity)
                _stackingSubEntities[(stackingSubEntity.Position, stackingSubEntity.SubPosition)] = stackingSubEntity;
        }

        private void Remove(IEntityData entityData)
        {
            _updatableEntities.Remove(entityData);
            _createdViews.Remove(entityData);
            if (entityData is IStackingSubEntity stackingSubEntity)
                _stackingSubEntities.Remove((stackingSubEntity.Position, stackingSubEntity.SubPosition));
        }
        
        private void OnEntityDestroyRequest(ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case DestroyCommand destroyCommand:
                        var data = destroyCommand.EntityData;
                        data.CommandRequest -= OnEntityDestroyRequest;
                        _context.SpatialMap.FreeTile(data.Position, data.EntityType);
                        Remove(data);
                        return;
                }
            }
            
        }
    }
}