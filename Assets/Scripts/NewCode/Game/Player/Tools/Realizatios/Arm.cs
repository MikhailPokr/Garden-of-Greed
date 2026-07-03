namespace Garden
{
    public class Arm : ITool
    {
        public ToolType Type => ToolType.Arm;

        private bool _isHandBusy;
        private FruitData _fruitData;
        private float _poisonMult;

        public Arm(float poisonMult)
        {
            _poisonMult = poisonMult;
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
                signal.Entity.EntityData.ForceUseCommands(new GrabCommand(signal.Entity.EntityData,  _poisonMult));
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