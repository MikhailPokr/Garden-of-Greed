using System;
using UnityEngine;

namespace Garden
{
    public interface IEntityData
    {
        public event Action<Sprite> ChangeSpriteRequest;
        public event Action WashColor;
        public event Action<IEntityData> DestroyRequest;

        public void Start();

        public void Update(float deltaTime);
    }
}