using System;
using UnityEngine;

namespace Garden
{
    public class FruitData : IEntityData
    {
        public readonly FruitDataConfig DataConfig;
        public event Action<Sprite> ChangeSpriteRequest;
        public event Action WashColor;
        public event Action<IEntityData> DestroyRequest;

        public FruitData(FruitDataConfig dataConfig)
        {
            DataConfig = dataConfig;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}