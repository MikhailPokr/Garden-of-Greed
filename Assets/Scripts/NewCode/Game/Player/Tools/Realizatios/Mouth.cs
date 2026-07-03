using System;
using UnityEngine;

namespace Garden
{
    public class Mouth : ITool
    {
        public ToolType Type =>  ToolType.Mouth;

        public void Activate()
        {
        }

        public void Process(IClickSignal signal) => OnEntityClicked((EntityClickSignal)signal);

        private void OnEntityClicked(EntityClickSignal signal)
        {
            signal.Entity.EntityData.ForceUseCommands(new EatCommand(signal.Entity.EntityData));
        }
    }
}