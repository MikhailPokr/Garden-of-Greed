using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace Garden
{
    public class CompositeRoot : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private ToolSelectorsController _toolSelectorsController;
        [SerializeField] private UIMoneyCounter _moneyCounter;
        
        private int _seed;
        private Player _player;
        private SpatialMap _spatialMap;
        private Field _field;
        private EntityCreationManager _creationManager;
        private GrassGenerator _grassGenerator;
        private ToolManager _toolManager;
        private InputManager _inputManager;
        private GenomeFactory _genomeFactory;
        private TreeShop _treeShop;
        private Arm _arm;
        private SaleTool _saleTool;
        private Scythe _scythe;
        
        Dictionary<EntityType, IEntityCreationController> _entityControllers;
        
        private void Awake()
        {
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;
            
            if (!_gameConfig.UseSeed)
                _seed = _gameConfig.UseSeed ? _gameConfig.Seed : SeedUtils.GenerateSeed();
            
            _player = new Player(_gameConfig.StartOptions);
            
            _spatialMap = new SpatialMap(_gameConfig.FieldOptions);

            _field = Instantiate(_gameConfig.FieldPrefab);
            _field.Init(_spatialMap, _gameConfig.GeneralPalette, _gameConfig.FieldOptions);
            
            _inputManager = new InputManager();
            
            _creationManager = new EntityCreationManager(
                new VisualContext(_gameConfig, _field, _spatialMap, _inputManager),
                _gameConfig.EntityBundles);
            
            _arm = new Arm();
            _treeShop = new TreeShop(_seed,  _player, 10);
            _saleTool = new SaleTool(_player);
            _scythe = new Scythe();
            
            List<ITool> tools = new List<ITool>()
            {
                _arm,
                _treeShop,
                _saleTool,
                _scythe
            };
            
            _toolManager = new ToolManager(_spatialMap, tools);
            
            _toolSelectorsController.Init(_toolManager);
            _moneyCounter.Init(_player);

            EntityBundle treeBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Tree);
            EntityBundle fruitBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Fruit);
            EntityBundle grassBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Grass);
            
            _grassGenerator = new GrassGenerator(_seed, grassBundle, _spatialMap);
            
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
                fruitBundle,
                _spatialMap,
                _genomeFactory,
                _player));
            _entityControllers.Add(EntityType.Grass, new GrassCreationController(
                _gameConfig.Seed,
                grassBundle));
            
            _player.Start();
        }

        private void Update()
        {
            float time = Time.deltaTime;
            _player.Update(time);
            _creationManager.Update(_player.Time);
            _grassGenerator.Update(_player.Time);
        }
    }
}
