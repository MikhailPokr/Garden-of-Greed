using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Garden
{
    public class FieldManager
    {
        public event Action<float> ColorChanged;
        public readonly GeneralPalette GeneralPalette;
        public readonly Camera Camera;
        public readonly Player Player;
        
        private readonly TreePalette _treePalette;
        
        private readonly EntityView _entityViewPrefab;
        private readonly Field _fieldPrefab;
        
        private readonly TreeFactory _treeFactory;
        private readonly FruitFactory _fruitFactory;
        private readonly GrassFactory _grassFactory;
        private readonly BerryFactory _berryFactory;
        
        private readonly OperationManager _operationManager;
        
        private Field _field;
        
        public List<IEntityData> _entities { get; }
        public FieldManager(
            EntityView entityView,
            Field field,
            GeneralPalette generalPalette,
            TreePalette treePalette,
            Player player,
            OperationManager operationManager,
            TreeGenerationOptions treeGenerationOptions,
            FruitGenerationOptions fruitGenerationOptions,
            BerryGenerationOptions berryGenerationOptions,
            GrassGenerationOptions grassGenerationOptions)
        {
            _entityViewPrefab = entityView;
            _fieldPrefab = field;
            GeneralPalette = generalPalette;
            _treePalette = treePalette;
            Player = player;
            _operationManager = operationManager;
            _treeFactory = new TreeFactory(treePalette,  treeGenerationOptions, Player);
            _fruitFactory = new FruitFactory(fruitGenerationOptions);
            _grassFactory = new GrassFactory(this.GeneralPalette, grassGenerationOptions);
            _berryFactory = new BerryFactory(this.GeneralPalette, berryGenerationOptions);
            _entities = new List<IEntityData>();
            Camera = Camera.main;
        }
        
        public void Update(float deltaTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(Player.Time);
            }
        }

        public void CreateField()
        {
            _field = Object.Instantiate(_fieldPrefab);
            _field.Init(this);

            for (int y = -5; y <= 7; y++)
            {
                for (int i = -2; i <= 2; i++)
                {
                    var o = CreateEntity(_treeFactory.Create(), _treePalette, new Vector2Int(i, y));
                }
            }
            
            
            
            ColorChanged?.Invoke(0);
        }


        private EntityView CreateEntity(IEntityData data, IPalette palette, Vector2Int position)
        {
            var entityView = Object.Instantiate(_entityViewPrefab);
            entityView.Init(data, this, palette, _field, position);
            _operationManager.RegisterEntity(entityView);
            _entities.Add(data);
            data.DestroyRequest += OnEntityDestroyRequest;
            data.Start();
            
            return entityView; 
        }
        
        private void OnEntityDestroyRequest(IEntityData data)
        {
            data.DestroyRequest -= OnEntityDestroyRequest;
            _entities.Remove(data);
        }
    }
}