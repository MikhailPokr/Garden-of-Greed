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

        public event Action<Sprite> ChangeSpriteRequest;
        public event Action WashColor;
        public event Action<IEntityData> DestroyRequest;
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
            if (_stage < DataConfig.StagesSprites.Count - 1)
            {
                ChangeSpriteRequest?.Invoke(DataConfig.StagesSprites[_stage]);
                _stage++;
                return;
            }
            
            if (_stage == DataConfig.StagesSprites.Count - 1)
            {
                ChangeSpriteRequest?.Invoke(DataConfig.StagesSprites[_stage]);
                WashColor?.Invoke();
                _stage++;
                return;
            }

            if (DataConfig.IsFruitTree && _stage <= DataConfig.LastFruitStage)
            {
                FruitRequest?.Invoke(this);
                _stage++;
                return;
            }
            
            if (_stage == DataConfig.MaxStage - 1)
            {
                if (DataConfig.AutoBreedingTree)
                    BreedRequest?.Invoke(this);
                _stage++;
                return;
            }

            if (_stage == DataConfig.MaxStage)
            {
                DryRequest?.Invoke();
                _timerEnabled = false;
                ChangeSpriteRequest?.Invoke(DataConfig.DieSprite);
            }

            _stage++;
        }
    }
}