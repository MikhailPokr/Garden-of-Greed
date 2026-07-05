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

        public void Process(InteractionData data)
        {
            data.EntityView.EntityData.ForceUseCommands(new EatCommand(data.EntityView.EntityData));
        }
    }
}