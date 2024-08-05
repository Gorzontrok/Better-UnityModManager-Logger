using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterUnityModManagerLogger
{
    [HarmonyPatch(typeof(UnityModManagerNet.UnityModManager.Logger))]
    public static class LoggerPatches
    {
        [HarmonyPatch("LogException", typeof(string), typeof(Exception), typeof(string))]
        [HarmonyPrefix]
        public static bool LogException(string key, Exception e, string prefix)
        {
            if (!Main.enabled || !Main.Loaded)
                return true;

            Traverse ummLoggerTraverse = Traverse.Create(typeof(UnityModManagerNet.UnityModManager.Logger));
            string logMessage = null;

            if (string.IsNullOrEmpty(key))
                logMessage = $"{prefix}{e.GetType().Name}";
            else
                logMessage = $"{prefix}{key}: {e.GetType().Name}";

            // Full exceptions or UnityModManager defaults ?
            if (Main.settings.writeFullException)
                logMessage = logMessage + $" - {e.ToString()}";
            else
                logMessage = logMessage + $" - {e.Message}";

            ummLoggerTraverse.Method("Write", logMessage, false).GetValue();

            Console.WriteLine(e.ToString());
            return false;
        }

        [HarmonyPatch("Write", typeof(string), typeof(bool))]
        [HarmonyPrefix]
        public static bool Write(string str, bool onlyNative)
        {
            if (!Main.enabled || !Main.Loaded)
                return true;

            if (str == null)
                return false;

            if (Main.settings.showLogTime)
            {
                str = str.Insert(0, Logs.GetCurrentTimePrefix() + ' ');
            }

            Console.WriteLine(str);

            if (onlyNative)
                return false;

            Traverse ummLoggerTraverse = Traverse.Create(typeof(UnityModManagerNet.UnityModManager.Logger));

            // buffer represent the log wrote on disk, inside 'Logs.txt'.
            List<string> buffer = ummLoggerTraverse.Field("buffer").GetValue<List<string>>();
            buffer.Add(str);

            AddLogToHistory(str, ummLoggerTraverse);

            return false;
        }

        private static void AddLogToHistory(string log, Traverse ummLoggerTraverse)
        {
            // history represent the logs shown in the "Logs" tab of UnityModManager window.
            List<string> history = ummLoggerTraverse.Field("history").GetValue<List<string>>();
            int historyCapacity = ummLoggerTraverse.Field("historyCapacity").GetValue<int>();

            if (Main.settings.useColor)
            {
                string color = Logs.GetColorHTML(log);
                if (!string.IsNullOrEmpty(color))
                {
                    log = Logs.AddColorXML(log, '#' + color);
                }
            }
            history.Add(log);
            if (Main.settings.screenLogs.showOnlyErrorAndException && Logs.IsError(log))
                ShowLogsOnScreen.Instance.AddLog(log);
            else if (!Main.settings.screenLogs.showOnlyErrorAndException)
                ShowLogsOnScreen.Instance.AddLog(log);

            if (history.Count >= historyCapacity * 2)
            {
                var result = history.Skip(historyCapacity);
                history.Clear();
                history.AddRange(result);
            }
        }
    }
}
