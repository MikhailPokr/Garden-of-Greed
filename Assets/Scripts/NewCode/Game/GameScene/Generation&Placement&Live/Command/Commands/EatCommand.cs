using UnityEngine;

namespace Garden
{
    public class EatCommand : DestroyCommand
    {
        public readonly int Hp;
        public EatCommand(IEntityData entityData) : base(entityData)
        {
            if (EntityData is FruitData fruitData)
                Hp = Mathf.RoundToInt(fruitData.DataConfig.TreeGenome.FruitLifeRegeneration);
        }

        public void Use()
        {
            SignalBus<ChangeHpSignal>.Fire(new (Hp));
        }
    }
}