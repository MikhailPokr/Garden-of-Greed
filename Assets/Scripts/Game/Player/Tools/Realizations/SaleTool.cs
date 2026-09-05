namespace Garden
{
    public class SaleTool : ITool
    {
        public ToolType Type => ToolType.Sale;
        public bool Locked { get; private set; }
        private readonly Player _player;
        
        public SaleTool(Player player)
        {
            _player = player;
        }
        
        public void Activate()
        {
        }
        public void Lock(bool locked)
        {
            Locked = locked;
        }

        public void Process(InteractionData data)
        {
            data.EntityView.EntityData.ForceUseCommands(new SaleCommand(data.EntityView.EntityData));
        }
    }
}