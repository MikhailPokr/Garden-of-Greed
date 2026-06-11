using System;

namespace Garden
{
    public static class SignalBus<T> where T : ISignal
    {
        public static event Action<T> OnEvent;
        public static void Fire(T signal) => OnEvent?.Invoke(signal); 
    }
}
