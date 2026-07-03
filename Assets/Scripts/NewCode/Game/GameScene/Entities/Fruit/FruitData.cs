using System;
using UnityEngine;

namespace Garden
{
    public class FruitData : IEntityData, IDependentEntity
    {
        public IEntityData HostEntity { get; }
        
        public readonly FruitDataConfig DataConfig;
        public TreeGenomeConfig TreeGenome => DataConfig.TreeGenome;
        
        private bool _timerEnabled;
        public int DropCount { get; private set; }
        
        public Vector2Int Position { get; private set; }
        public event Action<ICommand[]> CommandRequest;
        public CellType CellType => CellType.Free;
        public EntityType EntityType => EntityType.Fruit;
        
        public int Cost { get; private set; }

        public FruitData(TreeData treeData, FruitDataConfig dataConfig)
        {
            HostEntity = treeData;
            DataConfig = dataConfig;
            Position = HostEntity.Position;
            DropCount = 0;

            treeData.CommandRequest += OnHostCommand;
        }

        public void Start()
        {
            _timerEnabled = true;

            ProcessCommands(
                new ChangeCostCommand(Mathf.RoundToInt(DataConfig.GetCost())),
                new MarkChangesCommand());
        }

        public void Update(float currentTime)
        {
            if (!_timerEnabled)
                return;
            if (!DataConfig.IsRoting(currentTime)) return;
            DropCount++;
            ProcessCommands(new DestroyCommand(this));
        }
        
        public void ForceUseCommands(params ICommand[] commands) => ProcessCommands(commands);
        
        protected virtual void ProcessCommands(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case ChangeCostCommand changeCostCommand:
                        Cost = changeCostCommand.Value;
                        break;
                    case GrabCommand grabCommand:
                        grabCommand.Use();
                        Destroy();
                        break;
                    case SaleCommand saleCommand:
                        saleCommand.Use();
                        Destroy();
                        break;
                    case EatCommand eatCommand:
                        eatCommand.Use();
                        Destroy();
                        break;
                    case DestroyCommand:
                        Destroy();
                        break;
                }
            }

            CommandRequest?.Invoke(commands);
        }
        
        private void OnHostCommand(ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case SaleCommand saleCommand:
                        ProcessCommands(new SaleCommand(this));
                        break;
                    case DryCommand:
                    case DestroyCommand:
                        ProcessCommands(new DestroyCommand(this));
                        break;
                }
            }
        }
        
        private void Destroy()
        {
            HostEntity.CommandRequest -= OnHostCommand;
            _timerEnabled = false;
        }
    }
}