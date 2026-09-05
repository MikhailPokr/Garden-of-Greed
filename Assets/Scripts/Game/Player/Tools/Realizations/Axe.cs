namespace Garden
{
    public class Axe : ITool
    {
        public ToolType Type => ToolType.Axe;
        public bool Locked { get; private set; }
        private int _hpCost;

        public Axe(int hpCost)
        {
            _hpCost = hpCost;
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
            if (signal.EntityView.EntityData is TreeData tree)
            {
                tree.ForceUseCommands(new CutDownCommand(tree, _hpCost));
            }
        }
    }
}