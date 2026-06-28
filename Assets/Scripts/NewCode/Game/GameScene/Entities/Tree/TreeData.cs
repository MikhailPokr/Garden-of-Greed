using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class TreeData : IEntityData
    {
        public TreeDataConfig DataConfig;
        public TreeGenomeConfig TreeGenome => DataConfig.TreeGenomeConfig;
        
        protected int _stage;
        protected bool _timerEnabled;
        public int BreedCount { get; private set; }
        public int FruitCount { get; private set; }
        public bool IsPit { get; protected set; }
        public bool IsSprout => _stage < TreeGenome.LastGrowthStage;
        public Vector2Int? Position { get; private set; }
        public int Cost {get; private set;}
        public EntityType EntityType => EntityType.Tree;
        public event Action<ICommand[]> CommandRequest;
        private Queue<ICommand[]> _commandList;

        public TreeData(TreeDataConfig dataConfig)
        {
            DataConfig = dataConfig;
            _stage = 0;
            _timerEnabled = false;
            Position = null;
        }
        
        public void Start()
        {
            _timerEnabled = true;
            IsPit = true;
            _commandList = CreateList();
            ProcessCommands(_commandList.Dequeue());
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void Update(float currentTime)
        {
            if (!_timerEnabled)
                return;
            if (currentTime >= DataConfig.GetNextTimer(_stage))
            {
                ProcessCommands(_commandList.Dequeue());
            }
        }
        public void AddFruit(int fruitCount) => FruitCount += fruitCount;
        public void AddBreed(int breedCount) => BreedCount += breedCount;
        
        public void ForceUseCommands(params ICommand[] commands)
        {
            ProcessCommands(commands);
        }

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
                commandConfigurator.AddInRange(Mathf.RoundToInt(TreeGenome.LastGrowthStage) + 1, Mathf.RoundToInt(TreeGenome.MaxStage),
                    new FruitProduceCommand(this));
            if (!TreeGenome.TreeType.HasFlag(TreeType.Fruit))
                commandConfigurator.AddInPosition(Mathf.RoundToInt(TreeGenome.MaxStage) - 1, 
                    new BreedCommand(this));
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

        protected virtual void ProcessCommands(ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case ChangeCostCommand changeCostCommand:
                        Cost = changeCostCommand.Value;
                        break;
                    case SaleCommand saleCommand:
                        saleCommand.Use();
                        _timerEnabled = false;
                        break;
                    case DestroyCommand:
                        _timerEnabled = false;
                        break;
                    case CounterUpCommand:
                        _stage++;
                        break;
                    case DryCommand:
                        _timerEnabled = false;
                        break;
                    case FruitProduceCommand fruitProduceCommand:
                        fruitProduceCommand.Use();
                        break;
                }
            }

            CommandRequest?.Invoke(commands);
        }
        
        
    }
}