using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class TreeData : IEntityData
    {
        public CellType CellType => CellType.Main;
        public EntityType EntityType => EntityType.Tree;
        public Vector2Int Position { get; private set; }
        public int Cost {get; private set;}
        public event Action<ICommand[]> CommandRequest;

        public TreeDataConfig DataConfig { get; }
        public TreeGenomeConfig TreeGenome => DataConfig.TreeGenomeConfig;
        
        public int BreedCount { get; private set; }
        public int FruitCount { get; private set; }
        public int Stage { get; private set; }
        
        private bool _timerEnabled;
        private Queue<ICommand[]> _commandList;
        
        public TreeData(TreeDataConfig dataConfig, Vector2Int position)
        {
            DataConfig = dataConfig;
            Stage = 0;
            _timerEnabled = false;
            Position = position;
        }
        
        public void Start()
        {
            _timerEnabled = true;
            _commandList = CreateList();
            ProcessCommands(_commandList.Dequeue());
        }

        public void Update(float currentTime)
        {
            if (!_timerEnabled)
                return;
            float time = DataConfig.GetNextTimer(Stage, Position) - currentTime;
            if (time <= 0)
            {
                ProcessCommands(_commandList.Dequeue());
            }
            else if (time > DataConfig.DeadValue && Stage < TreeGenome.LastGrowthStage && Stage > 0)
            {
                ProcessCommands(
                    new ChangeSpriteCommand(TreeSpriteCommandsLegend.DeadShoot),
                    new DryCommand(),
                    new MarkChangesCommand());
            }
        }
        public void AddFruit(int fruitCount) => FruitCount += fruitCount;
        public void AddBreed(int breedCount) => BreedCount += breedCount;
        
        public void ForceUseCommands(params ICommand[] commands) => ProcessCommands(commands);

        protected virtual Queue<ICommand[]> CreateList()
        {
            CommandConfigurator commandConfigurator = new CommandConfigurator();
            
            commandConfigurator.AddInPosition(-1, 
                new ChangeSpriteCommand(TreeSpriteCommandsLegend.Pit),
                new ChangeColorCommand(TreeColorCommandsLegend.Pit),
                new MarkChangesCommand(),
                new ChangeCostCommand(0));
            for (int i = 0; i < TreeGenome.LastGrowthStage; i++)
            {
                commandConfigurator.AddInPosition(i,
                    new ChangeSpriteCommand(i));
            }
            commandConfigurator.AddInPosition(Mathf.RoundToInt(TreeGenome.LastGrowthStage),
                new ChangeSpriteCommand(TreeSpriteCommandsLegend.GrownTree),
            new ChangeColorCommand(TreeColorCommandsLegend.Tree),
                new ChangeCostCommand(Mathf.RoundToInt(TreeGenome.WoodCost)));
            commandConfigurator.AddInPosition(0,
                new ChangeColorCommand(TreeColorCommandsLegend.Sprout));
            commandConfigurator.AddInPosition(Mathf.RoundToInt(TreeGenome.LastGrowthStage));
            if (TreeGenome.TreeType.HasFlag(TreeType.Fruit))
                commandConfigurator.AddInRange(Mathf.RoundToInt(TreeGenome.LastGrowthStage) + 1, Mathf.RoundToInt(TreeGenome.LastFruitStage),
                    new FruitProduceCommand(this));
            if (!TreeGenome.TreeType.HasFlag(TreeType.Fruit))
                commandConfigurator.AddInPosition(Mathf.RoundToInt(TreeGenome.MaxStage) - 1, 
                    new BreedCommand(this));
            commandConfigurator.AddInPosition(Mathf.RoundToInt(TreeGenome.MaxStage) - 1, 
                new MarkChangesCommand());
            commandConfigurator.AddInPosition(Mathf.RoundToInt(TreeGenome.MaxStage),
                new DryCommand(),
                new MarkChangesCommand(),
                new ChangeCostCommand(Mathf.RoundToInt(TreeGenome.WoodCostDry)));
            commandConfigurator.AddInRange(0, Mathf.RoundToInt(TreeGenome.LastGrowthStage + 1), 
                new MarkChangesCommand());
            commandConfigurator.AddInRange(0,  Mathf.RoundToInt(TreeGenome.MaxStage),
                new CounterUpCommand());

            return commandConfigurator.GetCommands();
        }

        protected virtual void ProcessCommands(params ICommand[] commands)
        {
            for (var i = 0; i < commands.Length; i++)
            {
                var command = commands[i];
                switch (command)
                {
                    case ChangeCostCommand changeCostCommand:
                        Cost = changeCostCommand.Value;
                        break;
                    case CutDownCommand cutDownCommand:
                        if (Stage <= TreeGenome.LastGrowthStage)
                            commands[Array.IndexOf(commands, cutDownCommand)] = null;
                        else
                            cutDownCommand.Use();
                        break;
                    case MowCommand mowCommand:
                        if (Stage > TreeGenome.LastGrowthStage)
                            commands[Array.IndexOf(commands, mowCommand)] = null;
                        break;
                    case SaleCommand saleCommand:
                        saleCommand.Use();
                        _timerEnabled = false;
                        break;
                    case DestroyCommand:
                        _timerEnabled = false;
                        break;
                    case CounterUpCommand:
                        Stage++;
                        break;
                    case DryCommand:
                        _timerEnabled = false;
                        break;
                    case FruitProduceCommand fruitProduceCommand:
                        fruitProduceCommand.Use();
                        break;
                    case BreedCommand autoBreedSignal:
                        autoBreedSignal.Use();
                        break;
                }
            }

            CommandRequest?.Invoke(commands);
        }
        
        
    }
}