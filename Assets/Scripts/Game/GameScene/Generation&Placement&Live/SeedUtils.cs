using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Garden
{
    public static class SeedUtils
    {
        public static int GenerateSeed() => Random.Range(int.MinValue, int.MaxValue);

        public static int GetNewSeed(int seed, SeedUserType userType) => GetNewSeed(seed, (int)userType);

        public static int GetNewSeed(int seed, int salt)
        {
            uint h = (uint)(seed ^ salt);

            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return (int)h;
        }
        
        public static int GetRandom(int seed, ParamType salt, int max) => GetRandom(seed, (int)salt, 0, max);
        public static int GetRandom(int seed, ParamType salt, Vector2Int range) => GetRandom(seed, (int)salt, range.x, range.y);
        public static int GetRandom(int seed, int salt, Vector2Int range) => GetRandom(seed, salt, range.x, range.y);
        public static int GetRandom(int seed, int salt, int min, int max)
        {
            var range = max - min;
            if (range == 0)
                return min;

            uint state = (uint)seed + (uint)salt * 0x9E3779B9;

            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;

            return min + (int)(state % (uint)range);
        }
        public static float GetRandom(int seed, ParamType salt) => GetRandom(seed, (int)salt, 0f, 1f);
        public static float GetRandom(int seed, int salt) => GetRandom(seed, salt, 0f, 1f);
        public static float GetRandom(int seed, ParamType salt, Vector2 range) => GetRandom(seed, (int)salt, range.x, range.y);
        public static float GetRandom(int seed, int salt, Vector2 range) => GetRandom(seed, salt, range.x, range.y);
        public static float GetRandom(int seed, int salt, float min, float max)
        {
            uint state = (uint)seed + (uint)salt * 0x9E3779B9;

            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;

            var normalized = state / (float)uint.MaxValue;
    
            return min + normalized * (max - min);
        }

        public static T GetRandom<T>(int seed, ParamType salt, T list) where T : Enum
        {
            var a = Enum.GetValues(list.GetType());
            var b = GetRandom(seed, (int)salt, 0, a.Length);
            return (T)a.GetValue(b);
        }
    }
}