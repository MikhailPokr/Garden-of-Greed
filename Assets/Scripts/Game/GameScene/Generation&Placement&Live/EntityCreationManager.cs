using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class EntityCreationManager
    {
        private readonly Dictionary<EntityType, EntityBundle> _entityBundleLookup;
        private VisualContext _context;
        
        public List<IEntityData> CreatedEntities { get; private set; }
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
            
            CreatedEntities = new List<IEntityData>();
            _createdViews = new Dictionary<IEntityData, EntityView>();
            _stackingSubEntities = new Dictionary<(Vector2Int position, int subPosition), IStackingSubEntity>();

            SignalBus<EntityCreationSignal>.OnEvent += OnCreateEntity;
        }
        
        public void Update(float currentTime)
        {
            for (var i = 0; i < CreatedEntities.Count; i++)
            {
                CreatedEntities[i].Update(currentTime);
            }
        }

        private void OnCreateEntity(EntityCreationSignal signal)
        {
            CellData cell;
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
                    cell = new CellData(subEntity.CellType, subEntity.EntityType, subEntity.Position, subEntity.SubPosition);
                    entityView.gameObject.name = signal.EntityData.EntityType + $" ({subEntity.Position.x}:{subEntity.Position.y}|{subEntity.SubPosition})";
                    break;
                case { } entityData:
                    cell = new CellData(entityData.CellType, entityData.EntityType, entityData.Position);
                    entityView.gameObject.name = signal.EntityData.EntityType + $" ({entityData.Position.x}:{entityData.Position.y})";
                    break;
                default:
                    cell = null;
                    break;
            }
            _context.SpatialMap.OccupyTile(cell);
            
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
            
            CreatedEntities.Add(data);
            _createdViews[data] = entityView;
            
            if (data is IStackingSubEntity stackingSubEntity)
                _stackingSubEntities[(stackingSubEntity.Position, stackingSubEntity.SubPosition)] = stackingSubEntity;
        }

        private void Remove(IEntityData entityData)
        {
            CreatedEntities.Remove(entityData);
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
                        CellData cell;
                        switch (data)
                        {
                            case ISubEntity subEntity:
                                cell = new CellData(CellType.Null, data.EntityType, data.Position, subEntity.SubPosition);
                                break;
                            default:
                                cell = new CellData(CellType.Null, data.EntityType, data.Position);
                                break;
                        }
                        _context.SpatialMap.FreeTile(cell);
                        Remove(data);
                        return;
                    case CounterUpCommand upCommand:
                        if (upCommand.CellData != null)
                            _context.SpatialMap.OccupyTile(upCommand.CellData);
                        return;
                }
            }
            
        }
    }
}