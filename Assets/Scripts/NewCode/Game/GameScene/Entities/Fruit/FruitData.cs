using System;
using UnityEngine;

namespace Garden
{
    public class FruitData : IEntityData, IDependentEntity
    {
        public IEntityData HostEntity { get; }
        
        public FruitDataConfig DataConfig;
        public TreeGenomeConfig TreeGenome => DataConfig.TreeGenome;
        
        private bool _timerEnabled;
        public int DropCount { get; private set; }
        
        public Vector2Int? Position { get; private set; }
        public EntityType EntityType => EntityType.Fruit;
        public event Action<IEntityData> DestroyRequest;
        public event Action DropRequest;

        public FruitData(TreeData treeData, FruitDataConfig dataConfig)
        {
            HostEntity = treeData;
            DataConfig = dataConfig;
            if (HostEntity.Position != null)
                Position = HostEntity.Position;
            DropCount = 0;
            
            treeData.DryRequest += Destroy;
        }

        public void Start()
        {
            _timerEnabled = true;
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void Update(float currentTime)
        {
            if (!_timerEnabled)
                return;
            if (!DataConfig.IsRoting(currentTime)) return;
            DropCount++;
            Destroy();
        }

        public void Destroy()
        {
            _timerEnabled = false;
            DropRequest?.Invoke();
            DestroyRequest?.Invoke(this);
            ((TreeData)HostEntity).DryRequest -= Destroy;
        }
    }
}