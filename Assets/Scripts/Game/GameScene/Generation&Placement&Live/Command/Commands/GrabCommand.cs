using UnityEngine;

namespace Garden
{
    internal class GrabCommand : DestroyCommand
    {
        private readonly float _poisonMult;
        public GrabCommand(IEntityData entityEntityData, float poisonMult) : base(entityEntityData)
        {
            _poisonMult = poisonMult;
        }

        public void Use()
        {
            if (EntityData is FruitData fruitData)
            {
                if (fruitData.TreeGenome.FruitLifeRegeneration < 0)
                    SignalBus<ChangeHpSignal>.Fire(new (Mathf.RoundToInt( fruitData.TreeGenome.FruitLifeRegeneration * _poisonMult)));
            }
        }
    }
}