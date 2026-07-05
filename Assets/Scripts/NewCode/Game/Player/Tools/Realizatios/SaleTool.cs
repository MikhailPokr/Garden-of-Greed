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

        public void Process(InteractionData data)
        {
            if (_player.Money >= data.EntityView.EntityData.Cost)
            {
                data.EntityView.EntityData.ForceUseCommands(new SaleCommand(data.EntityView.EntityData));
            }
        }
    }
}