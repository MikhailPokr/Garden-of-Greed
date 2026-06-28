using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class DeadShootData : TreeData
    {
        public new EntityType EntityType => EntityType.DeadShoot;
        
        public DeadShootData(TreeDataConfig dataConfig) : base(dataConfig)
        {
        }

        protected override Queue<ICommand[]> CreateList()
        {
            CommandConfigurator commandConfigurator = new CommandConfigurator();
            commandConfigurator.AddInPosition(0, 
                new ChangeSpriteCommand(TreeSpriteCommandsLegend.Pit),
            new ChangeColorCommand(TreeColorCommandsLegend.Pit));
            commandConfigurator.AddInPosition(1, 
                new ChangeSpriteCommand(TreeSpriteCommandsLegend.DeadShoot),
                new ChangeColorCommand(TreeColorCommandsLegend.DeadShot),
                new DryCommand());
            commandConfigurator.AddInRange(0, 2,
                new CounterUpCommand(),
                new MarkChangesCommand());

            return commandConfigurator.GetCommands();
        }
    }
}