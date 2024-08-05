using UnityEngine;
using UnityModManagerNet;

namespace BetterUnityModManagerLogger
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Type = DrawType.Toggle, Label = "Use Color")]
        public bool useColor = true;
        [Draw(Type = DrawType.Toggle, Label = "Show Log Time")]
        public bool showLogTime = true;
        [Draw(Type = DrawType.Toggle, Label = "Write Full Exceptions")]
        public bool writeFullException = true;

        [Space(10), Draw("Screen Logs Settings", Collapsible = true)]
        public Settings.ScreenLogs screenLogs = new ScreenLogs();

        //[Space(10), Draw("Colors", Collapsible = true)]
        public Settings.Colors colors = new Settings.Colors();

        [Draw(Type = DrawType.Toggle, Label = "Show Debug Logs")]
        public bool showDebugLogs = false;

        public LogTab logTab = new LogTab();

        public void OnChange()
        { }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        [DrawFields(DrawFieldMask.Public)]
        public class ScreenLogs
        {
            public ScreenLogs()
            {
                keyToClearLogs = new KeyBinding();
                keyToClearLogs.Change(KeyCode.F7);
            }


            [Draw(Type = DrawType.Toggle, Label = "Enabled")]
            public bool enabled = false;

            [Draw(Type = DrawType.KeyBinding, Label = "Key To Clear Logs")]
            public KeyBinding keyToClearLogs = new KeyBinding();

            /// <summary> The number of seconds a log stay on screen </summary>
            [Draw(Type = DrawType.Slider, Label = "Logs Time", Min = 1, Max = 10)]
            public int logsTimeOnScreen = 5;

            [Draw(Type = DrawType.Toggle, Label = "Show only Errors and Exceptions")]
            public bool showOnlyErrorAndException = false;

            [Draw(Type = DrawType.Field, Label = "Font Size")]
            public int fontSize = 13;
        }

        [DrawFields(DrawFieldMask.Public)]
        public class Colors
        {

            [Draw(Type = DrawType.Field, Label = "Default Log Color", Width = 64)]
            public Color defaultLog = Color.white;
            [Draw(Type = DrawType.Field, Label = "Warning Log Color", Width = 64)]
            public Color warningLog = Color.yellow;
            [Draw(Type = DrawType.Field, Label = "Exception Log Color", Width = 64)]
            public Color exceptionLog = new Color32(255, 165, 0, 255);
            [Draw(Type = DrawType.Field, Label = "Error Log Color", Width = 64)]
            public Color errorLog = new Color32(225, 0, 0, 255);
            [Draw(Type = DrawType.Field, Label = "Critical Log Color", Width = 64)]
            public Color criticalLog = Color.red;
            [Draw(Type = DrawType.Field, Label = "Success Log Color", Width = 64)]
            public Color successLog = Color.green;
        }

        public class LogTab : IDrawable
        {
            [Draw(Type = DrawType.Toggle, Label = "Hide Basic Logs")]
            public bool hideBasicLogs = false;
            [Draw(Type = DrawType.Toggle, Label = "Hide Manager Logs")]
            public bool hideManagerLog = false;
            [Draw(Type = DrawType.Toggle, Label = "Hide Warning Logs")]
            public bool hideWarnings = false;
            [Draw(Type = DrawType.Toggle, Label = "Hide Error Logs")]
            public bool hideErrors = false;

            public void OnChange()
            { }
        }
    }
}
