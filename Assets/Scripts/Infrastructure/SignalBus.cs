using System;

namespace Garden
{
    public static class SignalBus<T> where T : ISignal
    {
        static SignalBus()
        {
            SignalBusCleaner.OnClearAll += Clear;
        }

        public static event Action<T> OnEvent;
        public static void Fire(T signal) => OnEvent?.Invoke(signal); 
        private static void Clear() => OnEvent = null; 
    }
}
