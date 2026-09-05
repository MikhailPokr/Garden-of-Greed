using System;
using UnityEngine;

namespace Garden
{
    public class BerryData : ISubEntity
    {
        public CellType CellType => CellType.Sub;
        public EntityType EntityType => EntityType.Berry;
        public BerryDataConfig DataConfig { get; }
        public Vector2Int Position { get; }
        public int SubPosition { get; }
        public int Cost { get; private set; }
        public event Action<ICommand[]> CommandRequest;
        
        public BerryData(BerryDataConfig dataConfig, Vector2Int position, int subPosition)
        {
            DataConfig = dataConfig;
            Position = position;
            SubPosition = subPosition;
        }

        public void Start()
        {
            ProcessCommand(
                new ChangeSpriteCommand(0),
                new ChangeColorCommand(0),
                new ChangeCostCommand(DataConfig.Cost),
                new MarkChangesCommand(),
                new CounterUpCommand(new CellData(CellType.Sub, EntityType.Berry, Position, SubPosition))
                );
        }
        

        public void Update(float deltaTime)
        {
        }

        public void ForceUseCommands(params ICommand[] commands) => ProcessCommand(commands);
        
        private void ProcessCommand(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case ChangeCostCommand changeCost:
                        Cost = DataConfig.Cost;
                        break;
                    case SaleCommand saleCommand:
                        saleCommand.Use();
                        break;
                    case EatCommand eatCommand:
                        eatCommand.Use();
                        break;
                }
            }

            CommandRequest?.Invoke(commands);
        }

    }
}