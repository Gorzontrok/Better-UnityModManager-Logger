using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityModManagerNet;

namespace BetterUnityModManagerLogger
{
    [HarmonyPatch(typeof(UnityModManager.UI))]
    public static class LogTabPatches
    {
        [HarmonyPatch("DrawTab")]
        [HarmonyPrefix]
        // The method was patched to add options to filter logs inside "Logs" tab
        public static bool DrawLogTab(UnityModManager.UI __instance, int tabId, UnityAction buttons)
        {
            if (!Main.enabled)
                return true;

            if (__instance.tabs[tabId] != "Logs")
                return true;

            Traverse uiTraverse = Traverse.Create(__instance);

            Settings.LogTab logTab = Main.settings.logTab;

            // Draw filter options
            GUILayout.BeginHorizontal();
            logTab.Draw(Main.mod);
            GUILayout.EndHorizontal();

            // Scroll Bar
            var minWidth = GUILayout.MinWidth(uiTraverse.Field("mWindowSize").GetValue<Vector2>().x);
            Vector2[] mScrollPosition = uiTraverse.Field("mScrollPosition").GetValue<Vector2[]>();
            mScrollPosition[tabId] = GUILayout.BeginScrollView(mScrollPosition[tabId], minWidth);

            // Logs
            GUILayout.BeginVertical("box");
            Traverse loggerTraverse = Traverse.Create(typeof(UnityModManager.Logger));
            List<string> history = loggerTraverse.Field("history").GetValue<List<string>>();
            int historyCapacity = loggerTraverse.Field("historyCapacity").GetValue<int>();

            for (int c = history.Count, i = Mathf.Max(0, c - historyCapacity); i < c; i++)
            {
                string log = history[i];
                if (logTab.hideManagerLog && log.Contains("[Manager]"))
                    continue;
                if (logTab.hideErrors && Logs.IsError(log))
                    continue;
                if (logTab.hideBasicLogs && !Logs.IsError(log) && !Logs.IsWarning(log))
                    continue;

                GUILayout.Label(history[i]);
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            // Buttons
            buttons += delegate
            {
                GUIStyle button = uiTraverse.Field("button").GetValue<GUIStyle>();
                if (GUILayout.Button("Clear", button, GUILayout.ExpandWidth(false)))
                {
                    loggerTraverse.Method("Clear").GetValue();
                }
                if (GUILayout.Button("Open detailed log", button, GUILayout.ExpandWidth(false)))
                {
                    uiTraverse.Method("OpenUnityFileLog").GetValue();
                }
            };

            return false;
        }
    }
}
