using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterUnityModManagerLogger
{
    public class ShowLogsOnScreen : MonoBehaviour
    {
        /// <summary> Instance of the Component. </summary>
        public static ShowLogsOnScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("BetterUnityModManagerLogger").AddComponent<ShowLogsOnScreen>();
                }
                return _instance;
            }
        }
        protected static ShowLogsOnScreen _instance;

        protected static Settings.ScreenLogs Setting { get => Main.settings.screenLogs; }
        public static bool IsEnabled { get => Setting.enabled; }

        protected List<KeyValuePair<string, DateTime>> _logs = new List<KeyValuePair<string, DateTime>>();

        /// <summary> Clear the logs shown on screen. </summary>
        public void ClearScreen()
        {
            _logs.Clear();
        }

        internal void AddLog(string log)
        {
            if (IsEnabled)
                _logs.Add(new KeyValuePair<string, DateTime>(log, DateTime.Now));
        }

        protected void Awake()
        {
            if (_instance != null)
            {
                Main.Logger.Warning("Second ShowLogsOnScreen created. Destroying the new one.");
                Destroy(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);

            _logs = new List<KeyValuePair<string, DateTime>>();
        }

        protected void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }

            Main.Logger.Warning($"{nameof(ShowLogsOnScreen)} destroyed");
        }

        protected void Update()
        {
            if (Setting.keyToClearLogs.Down())
                ClearScreen();

            // Check if a log should be removed
            List<KeyValuePair<string, DateTime>> temp = new List<KeyValuePair<string, DateTime>>();
            foreach (KeyValuePair<string, DateTime> pair in _logs)
            {
                if ((DateTime.Now.Second - pair.Value.Second) < Setting.logsTimeOnScreen)
                {
                    temp.Add(pair);
                }
            }
            _logs = temp;
        }

        protected void OnGUI()
        {
            if (!IsEnabled || _logs.Count < 1)
                return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = Main.settings.screenLogs.fontSize;

            GUILayout.BeginVertical("box");

            foreach (KeyValuePair<string, DateTime> pair in _logs)
            {
                GUILayout.Label(pair.Key, style);
            }
            GUILayout.EndVertical();
        }
    }
}