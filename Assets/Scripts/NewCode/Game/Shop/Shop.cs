using UnityEngine;

namespace Garden
{
    public class Shop
    {
        private int _seed;
        private int[] _currentSlots;
        private int _lastIndex;
        
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

        public int Get() => Get(0);

        public int Get(int slot)
        {
            var salt = _currentSlots[slot];
            SlotShift(slot);
            return SeedUtils.GetNewSeed(_seed, salt);
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