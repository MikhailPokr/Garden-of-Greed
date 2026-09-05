using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class BurnCommand : DestroyCommand
    {
        public BurnCommand(IEntityData entityData) : base(entityData)
        {
            
        }
    }
}