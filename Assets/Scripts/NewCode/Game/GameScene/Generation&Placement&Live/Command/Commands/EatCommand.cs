using UnityEngine;

namespace Garden
{
    public class EatCommand : DestroyCommand
    {
        public readonly int Hp;
        public EatCommand(IEntityData entityData) : base(entityData)
        {
            Hp = EntityData switch
            {
                FruitData fruitData => Mathf.RoundToInt(fruitData.DataConfig.TreeGenome.FruitLifeRegeneration),
                BerryData berryData => Mathf.RoundToInt(berryData.DataConfig.Regeneration),
                _ => Hp
            };
        }

        public void Use()
        {
            SignalBus<ChangeHpSignal>.Fire(new (Hp));
        }
    }
}