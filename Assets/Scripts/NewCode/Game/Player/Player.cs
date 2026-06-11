namespace Garden
{
    public class Player
    {
        public readonly PlayerStartOptions StartOptions;
        public int Money { get; private set; }
        public int Stamina { get; private set; }
        public float Time { get; private set; }

        public Player(PlayerStartOptions startOptions)
        {
            StartOptions = startOptions;
            
            Money = StartOptions.StartMoney;
            Stamina = StartOptions.MaxStamina;
        }

        public void Update(float deltaTime)
        {
            Time += deltaTime;
        }
    }
}