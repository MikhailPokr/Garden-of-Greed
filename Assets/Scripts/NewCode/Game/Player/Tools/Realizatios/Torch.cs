namespace Garden
{
    public class Torch : ITool
    {
        private readonly Player _player;
        public ToolType Type => ToolType.Torch;

        public Torch(Player player)
        {
            _player = player;
        }

        public void Activate()
        {
            
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