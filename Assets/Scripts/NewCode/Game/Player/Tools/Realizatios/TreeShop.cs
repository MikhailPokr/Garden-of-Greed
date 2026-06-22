using UnityEngine;

namespace Garden
{
    public class TreeShop : ITool
    {
        public ToolType Type => ToolType.TreeShop;
        
        private int _seed;
        private readonly Player _player;
        private int _count;

        private int _cost;
        
        public TreeShop(int globalSeed, Player player, int cost)
        {
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.Shop);
            _player = player;
            _cost = cost;
        }
        
        public void Activate()
        {
            
        }

        public void Process(IClickSignal signal) => OnFieldInteract((FieldClickSignal)signal);

        public bool Get(out int seed)
        {
            if (!_player.TryChangeMoney(-_cost))
            {
                seed = -1;
                return false;
            }

            seed = SeedUtils.GetNewSeed(_seed, _count++);
            return true;
        }
        
        private void OnFieldInteract(FieldClickSignal signal)
        {
            if (Get(out int seed))
                SignalBus<ArmPlantTreeSignal>.Fire(new ArmPlantTreeSignal(seed, signal.Position));
        }
    }
}