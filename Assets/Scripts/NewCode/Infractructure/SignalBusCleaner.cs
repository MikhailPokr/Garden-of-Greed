using System;

namespace Garden
{
    public static class SignalBusCleaner
    {
        public static event Action OnClearAll;
        
        public static void ClearAll() => OnClearAll?.Invoke();
    }
}