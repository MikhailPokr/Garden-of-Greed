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

        public void Process(InteractionData data)
        {
            if (data.EntityTarget)
                SetInArm(data);
            else
                PlaceFruit(data);
        }
            

        private void SetInArm(InteractionData data)
        {
            if (_isHandBusy)
                return;
            if (data.EntityView.EntityType == EntityType.Fruit)
            {
                _isHandBusy = true;
                _fruitData = data.EntityView.EntityData as FruitData;
                data.EntityView.EntityData.ForceUseCommands(new GrabCommand(data.EntityView.EntityData,  _poisonMult));
            }
        }

        private void PlaceFruit(InteractionData data)
        {
            if (!_isHandBusy)
                return;
            SignalBus<ArmPlantFruitSignal>.Fire(new ArmPlantFruitSignal(_fruitData, data.Position));
            _isHandBusy = false;
            _fruitData = null;
        }
    }
}