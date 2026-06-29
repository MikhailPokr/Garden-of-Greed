using System;
using UnityEngine;

namespace Garden
{
    public class BerryData : IEntityData
    {
        public EntityType EntityType => EntityType.Berry;

        public Vector2Int Position { get; }
        public event Action<ICommand[]> CommandRequest;

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

        public int Cost { get; }
        public void ForceUseCommands(params ICommand[] commands)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}