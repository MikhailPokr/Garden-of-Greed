using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class CompositeRoot : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        
        private int _seed;
        private Player _player;
        private FieldManager _fieldManager;
        private OperationManager _operationManager;
        private EntityCreationManager _creationManager;
        private Shop _shop;
        
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
                _player);
            
            _creationManager = new EntityCreationManager(
                _seed,
                _gameConfig.EntityBundles,
                _gameConfig.SpriteOrderOptions,
                _operationManager,
                _player,
                _fieldManager.CreateField());

            _shop = new Shop(_seed, 1);
            
            for (int y = -5; y <= 7; y++)
            {
                for (int i = -2; i <= 2; i++)
                {
                    SignalBus<EntityCreationRequestSignal>.Fire(new EntityCreationRequestSignal(EntityType.Tree, new Vector2Int(i, y)));
                }
            }
        }

        private void Update()
        {
            float time = Time.deltaTime;
            _player.Update(time);
            _creationManager.Update(time);
            
        }
    }
}
