using UnityEngine;

namespace Garden
{
    public class Arm
    {
        private readonly ISpatialMap _spatialMap;

        private bool _active;
        private EntityType _type;
        private int _seedInArm;
        
        public Arm(ISpatialMap spatialMap)
        {
            _spatialMap = spatialMap;
            
            SignalBus<FieldClickSignal>.OnEvent += OnFieldInteract;
            SignalBus<SetInArmSignal>.OnEvent += SetInArm;
        }

        public void SetInArm(SetInArmSignal signal)
        {
            _active = true;
            _type = signal.Type;
            _seedInArm = signal.Seed;
        }

        private void OnFieldInteract(FieldClickSignal signal)
        {
            if (!_active || signal.InteractionType != InteractionType.Click || !_spatialMap.IsTileFreeAndValid(signal.Position))
                return;

            SignalBus<ArmPlantTreeSignal>.Fire(new ArmPlantTreeSignal(_type, _seedInArm, signal.Position));
            
            _active = false;
        }
    }
}