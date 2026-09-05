using System;
using UnityEngine;

namespace Garden
{
    public class Mouth : ITool
    {
        public ToolType Type =>  ToolType.Mouth;
        public bool Locked { get; private set; }

        public void Activate()
        {
        }
        
        public void Lock(bool locked)
        {
            Locked = locked;
        }

        public void Process(InteractionData data)
        {
            data.EntityView.EntityData.ForceUseCommands(new EatCommand(data.EntityView.EntityData));
        }
    }
}