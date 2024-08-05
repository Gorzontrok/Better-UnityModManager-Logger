using HarmonyLib;
using System;
using UnityModManagerNet;

namespace BetterUnityModManagerLogger
{
    public static class ModLoggerExtensions
    {
        #region Debug
        private const string DEBUG = "[Debug] ";

        public static void Debug(this UnityModManager.ModEntry.ModLogger logger, string message)
        {
            if (!Main.settings.showDebugLogs)
                return;

            UnityModManager.Logger.Log(message, Traverse.Create(logger).Field("Prefix").GetValue<string>() + DEBUG);
        }
        public static void DebugError(this UnityModManager.ModEntry.ModLogger logger, string message)
        {
            if (!Main.settings.showDebugLogs)
                return;

            UnityModManager.Logger.Log(message, Traverse.Create(logger).Field("PrefixError").GetValue<string>() + DEBUG);
        }
        public static void DebugCritical(this UnityModManager.ModEntry.ModLogger logger, string message)
        {
            if (!Main.settings.showDebugLogs)
                return;

            UnityModManager.Logger.Log(message, Traverse.Create(logger).Field("PrefixCritical").GetValue<string>() + DEBUG);
        }
        public static void DebugWarning(this UnityModManager.ModEntry.ModLogger logger, string message)
        {
            if (!Main.settings.showDebugLogs)
                return;

            UnityModManager.Logger.Log(message, Traverse.Create(logger).Field("PrefixWarning").GetValue<string>() + DEBUG);
        }
        public static void DebugException(this UnityModManager.ModEntry.ModLogger logger, string message, Exception exception)
        {
            if (!Main.settings.showDebugLogs)
                return;

            UnityModManager.Logger.LogException(message, exception, Traverse.Create(logger).Field("PrefixException").GetValue<string>() + DEBUG);
        }
        public static void DebugException(this UnityModManager.ModEntry.ModLogger logger, Exception exception)
        {
            if (!Main.settings.showDebugLogs)
                return;

            UnityModManager.Logger.LogException(null, exception, Traverse.Create(logger).Field("PrefixException").GetValue<string>() + DEBUG);
        }
        #endregion

        public static void SuccessLog(this UnityModManager.ModEntry.ModLogger logger, string message)
        {
            const string SUCCESS = "[Success] ";
            UnityModManager.Logger.Log(message, Traverse.Create(logger).Field("Prefix").GetValue<string>() + SUCCESS);
        }
    }
}
