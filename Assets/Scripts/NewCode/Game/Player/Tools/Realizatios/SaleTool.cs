namespace Garden
{
    public class SaleTool : ITool
    {
        public ToolType Type => ToolType.Sale;
        private readonly Player _player;
        
        public SaleTool(Player player)
        {
            _player = player;
        }
        
        public void Activate()
        {
        }

        public void Process(IClickSignal signal) => OnEntityClicked((EntityClickSignal)signal);

        private void OnEntityClicked(EntityClickSignal signal)
        {
            if (_player.Money >= signal.Entity.EntityData.Cost)
            {
                signal.Entity.EntityData.ForceUseCommands(new SaleCommand(signal.Entity.EntityData));
            }
        }
    }
}