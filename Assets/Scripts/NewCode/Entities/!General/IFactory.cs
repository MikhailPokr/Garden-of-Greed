using UnityEngine;

namespace Garden
{
    public interface IFactory
    {
        const int FNV = 16777619;
        public static int GetRandom(int seed, ParamType salt, int max) => GetRandom(seed, salt, 0, max);
        public static int GetRandom(int seed, ParamType salt, Vector2Int range) => GetRandom(seed, salt, range.x, range.y);
        public static int GetRandom(int seed, ParamType salt, int min, int max)
        {
            seed = (seed ^ (int)salt) * FNV;
            
            var range = max - min;
            if (range == 0)
                return min;
            var num = (seed % range + range) % range + min;
            
            return num;
        }
        public static float GetRandom(int seed, ParamType salt, Vector2 range) => GetRandom(seed, salt, range.x, range.y);

        public static float GetRandom(int seed, ParamType salt, float min, float max)
        {
            seed = (seed ^ (int)salt) * FNV;
    
            var normalized = (uint)seed / (float)uint.MaxValue;
            
            return min + normalized * (max - min);
        }
    }
}