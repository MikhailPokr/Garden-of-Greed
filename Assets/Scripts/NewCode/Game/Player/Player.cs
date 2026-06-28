using System;

namespace Garden
{
    public class Player
    {
        public readonly PlayerStartOptions StartOptions;
        public int Money { get; private set; }
        //public int Stamina { get; private set; }
        public float Time { get; private set; }
        private ITool _currentTool;

        public Player(PlayerStartOptions startOptions)
        {
            StartOptions = startOptions;
            //Stamina = StartOptions.MaxStamina;
            SignalBus<ChangeMoneySignal>.OnEvent += OnChangeMoney;
            SignalBus<ChangeMoneySignal>.Fire(new ChangeMoneySignal(StartOptions.StartMoney));
        }

        public void Update(float deltaTime)
        {
            Time += deltaTime;
        }
        
        public void OnChangeMoney(ChangeMoneySignal signal) => Money += signal.Delta;
    }
}