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
        public event Action OnChangeMoney;

        public Player(PlayerStartOptions startOptions)
        {
            StartOptions = startOptions;
            //Stamina = StartOptions.MaxStamina;
            TryChangeMoney(StartOptions.StartMoney);
        }

        public void Update(float deltaTime)
        {
            Time += deltaTime;
        }
        
        public bool TryChangeMoney(int delta)
        {
            if (Money + delta < 0)
                return false; 
            Money += delta;
            OnChangeMoney?.Invoke();
            return true;
        }
    }
}