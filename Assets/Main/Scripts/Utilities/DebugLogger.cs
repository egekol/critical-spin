using UnityEngine;

namespace Main.Scripts.Utilities
{
    public static class DebugLogger
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}