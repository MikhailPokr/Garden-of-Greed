using System;
using UnityEngine;

namespace Garden
{
    public interface IEntityData
    {
        event Action<bool> SetColor;
        event Action<IEntityData> DestroyRequest;

        void Start();

        void Update(float deltaTime);
    }
}