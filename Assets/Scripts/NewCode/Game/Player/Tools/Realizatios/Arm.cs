using UnityEngine;

namespace Garden
{
    public class Arm : ITool
    {
        public ToolType Type => ToolType.Arm;

        private bool _isHandBusy;
        private FruitData _fruitData;
        
        public Arm()
        {
        }
        
        
        public void Activate()
        {
        }

        public void Process(IClickSignal signal)
        {
            switch (signal)
            {
                case FieldClickSignal fieldClickSignal:
                    PlaceFruit(fieldClickSignal);
                    break;
                case EntityClickSignal entityClickSignal:
                    SetInArm(entityClickSignal);
                    break;
            }
        }
            

        private void SetInArm(EntityClickSignal signal)
        {
            if (_isHandBusy)
                return;
            if (signal.Entity.EntityType == EntityType.Fruit)
            {
                _isHandBusy = true;
                _fruitData = signal.Entity.EntityData as FruitData;
                signal.Entity.EntityData.ForceUseCommands(new DestroyCommand(signal.Entity.EntityData));
            }
        }

        private void PlaceFruit(FieldClickSignal signal)
        {
            if (!_isHandBusy)
                return;
            SignalBus<ArmPlantFruitSignal>.Fire(new ArmPlantFruitSignal(_fruitData, signal.Position));
            _isHandBusy = false;
            _fruitData = null;
        }
    }
}