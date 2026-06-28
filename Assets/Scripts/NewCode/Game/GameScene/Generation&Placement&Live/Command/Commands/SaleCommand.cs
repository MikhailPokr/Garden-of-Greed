namespace Garden
{
    public class SaleCommand : DestroyCommand
    {
        public SaleCommand(IEntityData entityData) : base(entityData) {}

        public void Use()
        {
            SignalBus<ChangeMoneySignal>.Fire(new ChangeMoneySignal(EntityData.Cost));
        }
    }
}