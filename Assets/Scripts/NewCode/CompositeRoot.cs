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
        [SerializeField] private RectInt _bounds;
        
        private int _seed;
        private Player _player;
        private FieldManager _fieldManager;
        private Field _field;
        private OperationManager _operationManager;
        private EntityCreationManager _creationManager;
        private Shop _shop;
        private InputManager _inputManager;
        private Arm _arm;
        
        private void Awake()
        {
            if (!_gameConfig.UseSeed)
                _seed = _gameConfig.UseSeed ? _gameConfig.Seed : SeedUtils.GenerateSeed();
            
            _player = new Player(_gameConfig.StartOptions);
            _operationManager = new OperationManager();
            
            _fieldManager = new FieldManager(
                _seed,
                _gameConfig.FieldPrefab,
                _gameConfig.GeneralPalette,
                _player,
                _bounds);
            
            _field = _fieldManager.CreateField();
            
            _inputManager = new InputManager();
            
            _creationManager = new EntityCreationManager(
                _seed,
                new VisualContext(_gameConfig, _field, _inputManager),
                _gameConfig.EntityBundles,
                _operationManager,
                _player);

            _shop = new Shop(_seed, 1);
            _shopUI.Init(_shop);
            
            _arm = new Arm(_field, _creationManager, _bounds);
        }

        private void Update()
        {
            float time = Time.deltaTime;
            _player.Update(time);
            _creationManager.Update(time);
            _fieldManager.Update();
        }
    }
}
