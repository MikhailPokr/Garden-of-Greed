namespace Garden
{
    public class CutDownCommand : DestroyCommand
    {
        private int _hpCost;
        public CutDownCommand(IEntityData entityData, int cost) : base(entityData)
        {
            _hpCost = cost;
        }

        public void Use()
        {
            var tree = EntityData as TreeData;
            SignalBus<AddFireSignal>.Fire(new AddFireSignal(tree));
            SignalBus<ChangeHpSignal>.Fire(new ChangeHpSignal(-_hpCost));
        }
    }
}