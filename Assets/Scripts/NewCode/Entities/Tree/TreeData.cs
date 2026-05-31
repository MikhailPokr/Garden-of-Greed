using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class TreeData : IEntityData
    {
        public readonly TreeDataConfig DataConfig;
        
        private int _stage;
        private bool _timerEnabled;

        public event Action<bool> SetColor;
        public event Action<IEntityData> DestroyRequest;
        public event Action<int> GrowRequest;
        public event Action DryRequest;
        public event Action<TreeData> BreedRequest;
        public event Action<TreeData> FruitRequest;

        public TreeData(TreeDataConfig dataConfig)
        {
            DataConfig = dataConfig;
            _stage = 0;
            _timerEnabled = false;
        }
        
        public void Start()
        {
            _timerEnabled = true;
        }

        public void Update(float currentTime)
        {
            if (!_timerEnabled)
                return;
            if (currentTime >= DataConfig.GetNextTimer(_stage))
            {
                SetNextStage();
            }
        }
        
        public void GetCost() => DataConfig.GetCost(_stage);

        private void SetNextStage()
        {
            if (_stage <= DataConfig.LastGrowthStage)
            {
                GrowRequest?.Invoke(_stage < DataConfig.LastGrowthStage ? _stage : -1);
            }

            if (DataConfig.TreeType.HasFlag(TreeType.Fruit) && _stage > DataConfig.LastFruitStage + 1 && _stage <= DataConfig.LastFruitStage)
            {
                FruitRequest?.Invoke(this);
            }
            
            if (_stage == DataConfig.MaxStage - 1)
            {
                if (!DataConfig.TreeType.HasFlag(TreeType.Fruit))
                    BreedRequest?.Invoke(this);
            }

            if (_stage == DataConfig.MaxStage)
            {
                DryRequest?.Invoke();
                _timerEnabled = false;
            }

            _stage++;
        }
    }
}