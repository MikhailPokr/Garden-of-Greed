namespace Garden
{
    public class DestroyCommand : ICommand
    {
        public IEntityData EntityData { get; private set; }

        public DestroyCommand(IEntityData entityData)
        {
            EntityData = entityData;
        }
    }
}