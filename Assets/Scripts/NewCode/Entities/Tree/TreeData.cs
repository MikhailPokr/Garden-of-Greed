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
        
        public bool IsSprout => _stage <= DataConfig.LastGrowthStage;

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
            int processingStage = _stage;
            _stage++;
            
            if (processingStage <= DataConfig.LastGrowthStage)
            {
                GrowRequest?.Invoke(processingStage < DataConfig.LastGrowthStage ? processingStage : -1);
            }

            if (DataConfig.TreeType.HasFlag(TreeType.Fruit) && processingStage > DataConfig.LastFruitStage + 1 && processingStage <= DataConfig.LastFruitStage)
            {
                FruitRequest?.Invoke(this);
            }
            
            if (processingStage == DataConfig.MaxStage - 1)
            {
                if (!DataConfig.TreeType.HasFlag(TreeType.Fruit))
                    BreedRequest?.Invoke(this);
            }

            if (processingStage == DataConfig.MaxStage)
            {
                DryRequest?.Invoke();
                _timerEnabled = false;
            }
        }
    }
}