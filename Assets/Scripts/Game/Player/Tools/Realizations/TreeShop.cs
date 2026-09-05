using UnityEngine;

namespace Garden
{
    public class TreeShop : ITool
    {
        public ToolType Type => ToolType.TreeShop;
        public bool Locked { get; private set; }
        public readonly int Cost;

        private readonly int _seed;
        private readonly Player _player;
        private int _count;

        
        public TreeShop(int globalSeed, Player player, int cost)
        {
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.Shop);
            _player = player;
            Cost = cost;
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
            if (Get(out int seed))
                SignalBus<ArmPlantTreeSignal>.Fire(new ArmPlantTreeSignal(seed, data.Position));
        }

        public bool Get(out int seed)
        {
            if (_player.Money < Cost)
            {
                seed = -1;
                return false;
            }
            SignalBus<ChangeMoneySignal>.Fire(new ChangeMoneySignal(Cost));
            seed = SeedUtils.GetNewSeed(_seed, _count++);
            return true;
        }
    }
}