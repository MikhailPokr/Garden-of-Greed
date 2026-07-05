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

        public void Process(InteractionData data)
        {
            if (Get(out int seed))
                SignalBus<ArmPlantTreeSignal>.Fire(new ArmPlantTreeSignal(seed, data.Position));
        }

        public bool Get(out int seed)
        {
            if (_player.Money < _cost)
            {
                seed = -1;
                return false;
            }
            SignalBus<ChangeMoneySignal>.Fire(new ChangeMoneySignal(_cost));
            seed = SeedUtils.GetNewSeed(_seed, _count++);
            return true;
        }
    }
}