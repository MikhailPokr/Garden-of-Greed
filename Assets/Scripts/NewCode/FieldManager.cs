using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Garden
{
    public class FieldManager
    {
        public event Action<float> ColorChanged;
        public readonly Palette Palette;
        public readonly Camera Camera;
        public readonly Player Player;
        
        private readonly EntityView _entityViewPrefab;
        private readonly DinamicField _dinamicFieldPrefab;

        
        private readonly TreeFabric _treeFabric;
        private readonly FruitFabric _fruitFabric;
        private readonly GrassFabric _grassFabric;
        private readonly BerryFabric _berryFabric;
        
        private readonly OperationManager _operationManager;
        
        private DinamicField _dinamicField;
        
        public List<IEntityData> _entities { get; }

        public Color EvilColor => Palette.EvilColor;
        public FieldManager(
            EntityView entityView,
            DinamicField dinamicField,
            Palette palette,
            Player player,
            OperationManager operationManager,
            TreeGenerationOptions treeGenerationOptions,
            FruitGenerationOptions fruitGenerationOptions,
            BerryGenerationOptions berryGenerationOptions,
            GrassGenerationOptions grassGenerationOptions)
        {
            _entityViewPrefab = entityView;
            _dinamicFieldPrefab = dinamicField;
            Palette = palette;
            Player = player;
            _operationManager = operationManager;
            _treeFabric = new TreeFabric(Palette, treeGenerationOptions, Player);
            _fruitFabric = new FruitFabric(fruitGenerationOptions);
            _grassFabric = new GrassFabric(Palette, grassGenerationOptions);
            _berryFabric = new BerryFabric(Palette, berryGenerationOptions);
            _entities = new List<IEntityData>();
            Camera = Camera.main;
        }
        
        public void Update(float deltaTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(Player.Time);
            }

            _dinamicField?.Check();
        }

        public void CreateField()
        {
            _dinamicField = Object.Instantiate(_dinamicFieldPrefab);
            _dinamicField.Init(this);

            CreateEntity(_treeFabric.Create(), true);
            
            ColorChanged?.Invoke(0);
        }


        private void CreateEntity(IEntityData data, bool needChangeColor)
        {
            var entityView = Object.Instantiate(_entityViewPrefab);
            entityView.Init(data, this, _dinamicField, needChangeColor);
            _operationManager.RegisterEntity(entityView);
            _entities.Add(data);
            data.DestroyRequest += OnEntityDestroyRequest;
            data.Start();
        }
        
        private void OnEntityDestroyRequest(IEntityData data)
        {
            data.DestroyRequest -= OnEntityDestroyRequest;
            _entities.Remove(data);
        }
    }
}