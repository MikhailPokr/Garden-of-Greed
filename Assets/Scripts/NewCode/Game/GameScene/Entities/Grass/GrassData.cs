using System;
using UnityEngine;

namespace Garden
{
    public class GrassData : IStackingSubEntity
    {
        public EntityType EntityType => EntityType.Grass;
        public GrassDataConfig DataConfig { get; }
        public Vector2Int Position { get; private set; }
        public int SubPosition { get; private set; }
        public int Cost { get; private set; }
        public event Action<ICommand[]> CommandRequest;
        
        private int _stage;
        private float _timer;
        
        
        public GrassData(GrassDataConfig dataConfig, Vector2Int position, int subPosition)
        {
            DataConfig = dataConfig;
            Position = position;
            SubPosition = subPosition;
        }

        public void Start()
        {
            Grow();
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < DataConfig.GrowTime)
                return;
            _timer = _timer - DataConfig.GrowTime;
            Grow();
        }
        public void Grow()
        {
            ProcessCommand(
                new ChangeSpriteCommand(_stage),
                new MarkChangesCommand(),
                new CounterUpCommand());
        }
        public void ForceUseCommands(params ICommand[] commands) => ProcessCommand(commands);

        private void ProcessCommand(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case CounterUpCommand:
                        _stage++;
                        break;
                }
            }
            
            CommandRequest?.Invoke(commands);
        }

    }
}