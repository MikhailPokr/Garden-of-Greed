using System;
using UnityEngine;

namespace Garden
{
    public class FruitData : IEntityData, IDependentEntity
    {
        public IEntityData HostEntity { get; }
        public readonly FruitDataConfig DataConfig;
        public event Action<Sprite> ChangeSpriteRequest;
        public event Action<bool> SetColor;
        public EntityType EntityType => EntityType.Fruit;
        public event Action<IEntityData> DestroyRequest;

        public FruitData(TreeData treeData, FruitDataConfig dataConfig)
        {
            HostEntity = treeData;
            DataConfig = dataConfig;
        }


        public Vector2Int? Position { get; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }

    }
}