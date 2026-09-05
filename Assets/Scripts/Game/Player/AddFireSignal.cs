using UnityEngine;

namespace Garden
{
    public struct AddFireSignal : ISignal
    {
        public bool IsEvil;
        public int FuelForce;

        public AddFireSignal(TreeData data)
        {
            IsEvil = data.TreeGenome.TreeType.HasFlag(TreeType.Evil);
            FuelForce = Mathf.RoundToInt(data.TreeGenome.FuelForce);
        }

        public AddFireSignal(int fuelForce)
        {
            IsEvil = false;
            FuelForce = fuelForce;
        }
    }
}