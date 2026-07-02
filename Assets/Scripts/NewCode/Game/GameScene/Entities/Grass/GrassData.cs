using System;
using UnityEngine;

namespace Garden
{
    public class GrassData : IStackingSubEntity
    {
        public CellType CellType => CellType.Sub;
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
            
            _stage = 0;
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
            _timer -= DataConfig.GrowTime;
            Grow();
        }
        public void Grow()
        {
            if (DataConfig.MaxStage <= _stage)
                return;
            ProcessCommand(
                new ChangeSpriteCommand(_stage),
                new ChangeColorCommand(0),
                new MarkChangesCommand(),
                new CounterUpCommand(new CellData(CellType.Sub, EntityType.Grass, Position, SubPosition)));
        }
        public void ForceUseCommands(params ICommand[] commands) => ProcessCommand(commands);

        private void ProcessCommand(params ICommand[] commands)
        {
            for (var i = 0; i < commands.Length; i++)
            {
                var command = commands[i];
                switch (command)
                {
                    case CounterUpCommand:
                        _stage++;
                        break;
                    case SaleCommand:
                        commands[i] = null;
                        break;
                }
            }

            CommandRequest?.Invoke(commands);
        }

    }
}