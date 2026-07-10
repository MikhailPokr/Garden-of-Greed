namespace Garden
{
    public class Torch : ITool
    {
        public ToolType Type => ToolType.Torch;
        public bool Locked { get; private set; }
        
        private readonly Player _player;

        public Torch(Player player)
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

        public void Process(InteractionData signal)
        {
            if (_player.FireData.Count == 0)
                return;
            if (signal.EntityView.EntityData is TreeData tree)
            {
                if (tree.Stage < tree.TreeGenome.LastGrowthStage)
                    return;
                tree.ForceUseCommands(new BurnCommand(tree));
                SignalBus<BurnSignal>.Fire(new BurnSignal(tree));
            }
        }
    }
}