using UnityEngine;

namespace Garden
{
    public class Shop
    {
        private int _seed;
        private int[] _currentSlots;
        public int _lastIndex;
        public Shop(int globalSeed, int slotsCount)
        {
            _seed = SeedUtils.GetNewSeed(globalSeed, SeedUserType.Shop);
            _currentSlots = new int[slotsCount];
            for (var i = 0; i < _currentSlots.Length; i++)
            {
                _currentSlots[i] = i;
            }
            _lastIndex = slotsCount;
        }

        public EntityCreationRequestSignal Get() => Get(0);

        public EntityCreationRequestSignal Get(int slot)
        {
            var item = new EntityCreationRequestSignal(EntityType.Tree, SeedUtils.GetNewSeed(_seed, slot), Vector2Int.zero);
            SlotShift(slot);
            return item;
        }

        private void SlotShift(int slot)
        {
            for (var i = slot; i < _currentSlots.Length - 1; i++)
            {
                _currentSlots[i] = _currentSlots[i + 1];
            }
            _currentSlots[^1] = _lastIndex;
            _lastIndex++;
        }
    }
}