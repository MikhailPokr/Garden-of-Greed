using System;

namespace Garden
{
    [Flags]
    public enum TreeType
    {
        Fruit = 1 << 0,
        Evil = 2 << 1,
    }
}