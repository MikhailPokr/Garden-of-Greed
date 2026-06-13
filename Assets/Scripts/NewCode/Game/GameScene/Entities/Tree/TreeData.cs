using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class TreeData : IEntityData
    {
        
        private readonly TreeDataConfig _dataConfig;
        public TreeGenomeConfig TreeGenome => _dataConfig.TreeGenomeConfig;
        
        private int _stage;
        private bool _timerEnabled;
        public int BreedCount { get; private set; }
        public int FruitCount { get; private set; }

        
        public bool IsSprout => _stage <= TreeGenome.LastGrowthStage;
        public Vector2Int? Position { get; private set; }
        public EntityType EntityType => EntityType.Tree;
        public event Action<IEntityData> DestroyRequest;
        public event Action<int> GrowRequest;
        public event Action DryRequest;
        public TreeData(TreeDataConfig dataConfig)
        {
            _dataConfig = dataConfig;
            _stage = 0;
            _timerEnabled = false;
            Position = null;
        }
        
        public void Start()
        {
            _timerEnabled = true;
            GrowRequest?.Invoke(0);
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void Update(float currentTime)
        {
            if (!_timerEnabled)
                return;
            if (currentTime >= _dataConfig.GetNextTimer(_stage))
            {
                SetNextStage();
            }
        }
        
        public void GetCost() => _dataConfig.GetCost(_stage);

        public void AddFruit(int fruitCount) => FruitCount += fruitCount;
        public void AddBreed(int breedCount) => BreedCount += breedCount;
            

        private void SetNextStage()
        {
            Debug.Log(_stage);
            
            if (_stage <= TreeGenome.LastGrowthStage)
            {
                GrowRequest?.Invoke(_stage < TreeGenome.LastGrowthStage ? _stage : -1);
            }

            if (TreeGenome.TreeType.HasFlag(TreeType.Fruit) && _stage > TreeGenome.LastFruitStage + 1 && _stage <= TreeGenome.LastFruitStage)
            {
                SignalBus<FruitProduceSignal>.Fire(new FruitProduceSignal(this));
            }
            
            if (_stage == Mathf.RoundToInt(TreeGenome.MaxStage) - 1)
            {
                if (!TreeGenome.TreeType.HasFlag(TreeType.Fruit))
                    SignalBus<AutoBreedSignal>.Fire(new AutoBreedSignal(this));
            }

            if (_stage == Mathf.RoundToInt(TreeGenome.MaxStage))
            {
                DryRequest?.Invoke();
                _timerEnabled = false;
            }
            
            _stage++;
        }
    }
}