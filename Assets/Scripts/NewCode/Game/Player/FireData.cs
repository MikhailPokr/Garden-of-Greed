using System;

namespace Garden
{
    public class FireData
    {
        public readonly bool IsEvilFire;
        public float Time;
        public event Action<FireData> TimeIsOver;

        public FireData(float time, bool isEvilFire)
        {
            Time = time;
            IsEvilFire = isEvilFire;
        }
        
        public void Update(float deltaTime)
        {
            Time -= deltaTime;
            if (Time <= 0)
                TimeIsOver?.Invoke(this);
        }
    }
}