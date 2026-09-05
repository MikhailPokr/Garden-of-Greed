using System;
using UnityEngine;

namespace Garden
{
    [Serializable]
    public class FireOptions
    {
        [field: SerializeField] public float FireTimePerPoint { get; private set; }
        [field: SerializeField] public float NormalFireMultiplier { get; private set; }
        [field: SerializeField] public float EvilFireTimeMultiplier { get; private set; }
    }
}