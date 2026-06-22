using UnityEngine;

namespace Garden
{
    public class Arm : ITool
    {
        public ToolType Type => ToolType.Arm;

        private bool _active;
        private EntityType _type;
        private int _seedInArm;
        
        public Arm()
        {
            SignalBus<SetInArmSignal>.OnEvent += SetInArm;
        }
        
        
        public void Activate()
        {
        }

        public void Process(IClickSignal signal)
        {
            
        }

        public void SetInArm(SetInArmSignal signal)
        {
            _active = true;
            _type = signal.Type;
            _seedInArm = signal.Seed;
        }

        

        
    }
}