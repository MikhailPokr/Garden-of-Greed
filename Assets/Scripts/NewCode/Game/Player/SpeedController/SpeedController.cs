using System;

namespace Garden
{
    public class SpeedController
    {
        public int CurrentSpeed { get; private set; }
        public int[] Speeds { get; }

        public event Action SpeedChange;

        public SpeedController()
        {
            Speeds = new[] { 0, 1, 2, 5 };
            CurrentSpeed = 1;
            
            SignalBus<TimeSpeedChangedSignal>.OnEvent += OnTimeSpeedChanged;
        }

        public void SetSpeed(int speed)
        {
            CurrentSpeed = speed;
            SpeedChange?.Invoke();
        }

        private void OnTimeSpeedChanged(TimeSpeedChangedSignal signal)
        {
            if (signal.Value == 1 && CurrentSpeed != Speeds[^1])
            {
                CurrentSpeed = Speeds[Array.IndexOf(Speeds, CurrentSpeed) + 1];
                SpeedChange?.Invoke();
                return;
            }

            if (signal.Value == -1 && CurrentSpeed != Speeds[0])
            {
                CurrentSpeed = Speeds[Array.IndexOf(Speeds, CurrentSpeed) - 1];
                SpeedChange?.Invoke();
                return;
            }
        }
    }
}