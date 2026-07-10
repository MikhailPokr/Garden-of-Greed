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
        [SerializeField] private UIHpCounter _hpCounter;
        [SerializeField] private UIFireCounter _fireCounter;
        [SerializeField] private UITimer _timer;
        [SerializeField] private SpeedButtonsController _speedButtonsController;
        [SerializeField] private RestartButton _restartButton;
        
        private int _seed;
        private Player _player;
        private SpatialMap _spatialMap;
        private Field _field;
        private EntityCreationManager _creationManager;
        private SubCellContentGenerator _subCellContentGenerator;
        private ToolManager _toolManager;
        private InputManager _inputManager;
        private GenomeFactory _genomeFactory;
        private TreeShop _treeShop;
        private Arm _arm;
        private SaleTool _saleTool;
        private Scythe _scythe;
        private GeoMap _geoMap;
        private Mouth _mouth;
        private Axe _axe;
        private Torch _torch;
        private ArsonManager _arsonManager;
        private SpeedController _speedController;
        private RestartController _restartController;
        private FenceGenerator _fenceGenerator;
        
        Dictionary<EntityType, IEntityCreationController> _entityControllers;
        
        private void Awake()
        {
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;
            
            if (!_gameConfig.UseSeed)
                _seed = _gameConfig.UseSeed ? _gameConfig.Seed : SeedUtils.GenerateSeed();
            
            _player = new Player(_gameConfig.StartOptions, _gameConfig);
            
            _spatialMap = new SpatialMap(_gameConfig.FieldOptions);
            

            _field = Instantiate(_gameConfig.FieldPrefab);
            _field.Init(_spatialMap, _gameConfig.GeneralPalette, _gameConfig.FieldOptions);
            _fenceGenerator = new FenceGenerator(_spatialMap, _gameConfig.GeneralPalette);
            _fenceGenerator.GenerateFence();
            
            _inputManager = new InputManager();
            
            _speedController = new SpeedController();
            _restartController = new RestartController();
            
            VisualContext visualContext = new VisualContext(_gameConfig, _field, _spatialMap, _inputManager); 
            
            _creationManager = new EntityCreationManager(
                visualContext,
                _gameConfig.EntityBundles);
            
            _geoMap = new GeoMap(visualContext);
            _arsonManager = new ArsonManager(_seed, _spatialMap, _creationManager, _gameConfig.ArsonOptions.Interval, _gameConfig.ArsonOptions.Chance);
            
            _arm = new Arm(0.5f);
            _treeShop = new TreeShop(_seed,  _player, -10);
            _saleTool = new SaleTool(_player);
            _scythe = new Scythe(1);
            _mouth = new Mouth();
            _axe = new Axe(1);
            _torch = new Torch(_player);
            
            List<ITool> tools = new List<ITool>()
            {
                _arm,
                _treeShop,
                _saleTool,
                _scythe,
                _mouth,
                _axe,
                _torch
            };
            
            _toolManager = new ToolManager(_spatialMap, _speedController, _player, tools);
            
            _toolSelectorsController.Init(_toolManager);
            _moneyCounter.Init(_player);
            _hpCounter.Init(_player);
            _fireCounter.Init(_player);
            _timer.Init(_player);
            _speedButtonsController.Init(_speedController);
            _restartButton.Init(_restartController, _speedController);
            

            EntityBundle treeBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Tree);
            EntityBundle fruitBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Fruit);
            EntityBundle grassBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Grass);
            EntityBundle berryBundle = _gameConfig.EntityBundles.Find(x => x.EntityType == EntityType.Berry);
            
            _subCellContentGenerator = new SubCellContentGenerator(_seed, grassBundle, berryBundle, _spatialMap);
            
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
                _geoMap,
                _player));
            _entityControllers.Add(EntityType.Fruit, new FruitCreationController(
                fruitBundle,
                _spatialMap,
                _genomeFactory,
                _player));
            _entityControllers.Add(EntityType.Grass, new GrassCreationController(
                _gameConfig.Seed,
                grassBundle));
            _entityControllers.Add(EntityType.Berry, new BerryCreationManager(
                _gameConfig.Seed,
                berryBundle,
                _spatialMap));
            
            _player.Start();
        }

        private void Update()
        {
            float time = Time.deltaTime * _speedController.CurrentSpeed;
            _player.Update(time);
            _creationManager.Update(_player.Time);
            _subCellContentGenerator.Update(_player.Time);
            _arsonManager.Update(time);
            _fireCounter.UpdateLogic();
            _timer.UpdateView();
        }
    }
}
