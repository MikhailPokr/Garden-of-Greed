using UnityEngine;

namespace Garden
{
    public class MowCommand : DestroyCommand
    {
        public MowCommand(IEntityData entityData) : base(entityData)
        {
        }
    }
}