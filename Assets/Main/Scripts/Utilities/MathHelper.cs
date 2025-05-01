using System;
using System.Collections.Generic;

namespace Main.Scripts.Utilities
{
    public static class MathHelper
    {
        private static readonly Random _random = new();

        public static float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }
        public static float Map(int value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }
        public static int GetRandomIndex<T>(List<T> list)
        {
            return _random.Next(0, list.Count); // Returns a random index in range [0, list.Count - 1]
        }
        public static int GetRandomIndex<T>(T[] array)
        {
            return _random.Next(0, array.Length); // Returns a random index in range [0, array.Length - 1]
        }

        public static float GetRandomValue(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}