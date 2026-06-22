using System;
using UnityEngine;

namespace Garden
{
    public interface IEntityData
    {
        EntityType EntityType { get; }
        event Action<IEntityData> DestroyRequest;
        public Vector2Int? Position { get; }

        void Start();

        void Update(float currentTime);
        int Cost { get; }

        public void Destroy();
    }
}