using UnityEngine;

namespace Garden
{
    [System.Serializable]
    public class ArsonOptions
    {
        public float Interval;
        [Range(0, 1)] public float Chance;
    }
}