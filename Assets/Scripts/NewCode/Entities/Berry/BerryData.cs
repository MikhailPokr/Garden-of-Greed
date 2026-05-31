using System;
using UnityEngine;

namespace Garden
{
    public class BerryData : IEntityData
    {
        public event Action<Sprite> ChangeSpriteRequest;
        public event Action<bool> SetColor;
        public event Action<IEntityData> DestroyRequest;
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