using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class CompositeRoot : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private ShopUI _shopUI;
        
        private int _seed;
        private Player _player;
        private SpatialMap _spatialMap;
        private Field _field;
        private EntityCreationManager _creationManager;
        private OperationManager _operationManager;
        private Shop _shop;
        private InputManager _inputManager;
        private Arm _arm;
        private GenomeFactory _genomeFactory;
        Dictionary<EntityType, IEntityCreationController> _entityControllers;
        
        private void Awake()
        {
            if (!_gameConfig.UseSeed)
                _seed = _gameConfig.UseSeed ? _gameConfig.Seed : SeedUtils.GenerateSeed();
            
            _player = new Player(_gameConfig.StartOptions);
            
            _operationManager = new OperationManager();
            
            _spatialMap = new SpatialMap(_gameConfig.FieldOptions);

            _field = Instantiate(_gameConfig.FieldPrefab);
            _field.Init(_spatialMap, _gameConfig.GeneralPalette, _gameConfig.FieldOptions);
            
            _inputManager = new InputManager();
            
            _creationManager = new EntityCreationManager(
                new VisualContext(_gameConfig, _field, _spatialMap, _inputManager),
                _gameConfig.EntityBundles,
                _operationManager,
                _player);

            _shop = new Shop(_seed, 1);
            _shopUI.Init(_shop);
            
            _arm = new Arm(_spatialMap);

            EntityBundle treeBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Tree);
            EntityBundle fruitBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Fruit);
            
            _genomeFactory = new GenomeFactory(
                treeBundle,
                fruitBundle,
                _gameConfig.MutationOptions);

            _entityControllers = new Dictionary<EntityType, IEntityCreationController>();
            _entityControllers.Add(EntityType.Tree, new TreeCreationController(
                _gameConfig.Seed,
                treeBundle,
                _spatialMap,
                _genomeFactory,
                _player));
            _entityControllers.Add(EntityType.Fruit, new FruitCreationController(
                _gameConfig.Seed,
                fruitBundle,
                _spatialMap,
                _genomeFactory,
                _player));
        }

        private void Update()
        {
            float time = Time.deltaTime;
            _player.Update(time);
            _creationManager.Update();
            _field.UpdateLogic();
        }
    }
}
