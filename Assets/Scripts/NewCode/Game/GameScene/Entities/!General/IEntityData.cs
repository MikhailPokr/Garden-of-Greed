using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public interface IEntityData
    {
        EntityType EntityType { get; }
        public Vector2Int? Position { get; }

        event Action<ICommand[]> CommandRequest;
        void Start();

        void Update(float currentTime);
        int Cost { get; }
        void ForceUseCommands(params ICommand[] commands);
    }
}