using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace BetterUnityModManagerLogger
{
    internal static class Main
    {
        public static bool Loaded { get; private set; }

        public static UnityModManager.ModEntry.ModLogger Logger => mod.Logger;

        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        private static bool _showColorOption = false;
        private static Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);

            var harmony = new Harmony(modEntry.Info.Id);
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            Loaded = true;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Color");
            _showColorOption = GUILayout.Toggle(_showColorOption, _showColorOption ? "Hide" : "Show", GUI.skin.button);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (!_showColorOption)
            {
                GUILayout.Space(10);
                return;
            }

            GUILayout.BeginVertical("box");
            MakeColorOption("Default Log", ref settings.colors.defaultLog);
            MakeColorOption("Warning Log", ref settings.colors.warningLog);
            MakeColorOption("Exception Log", ref settings.colors.exceptionLog);
            MakeColorOption("Error Log", ref settings.colors.errorLog);
            MakeColorOption("Critical Log", ref settings.colors.criticalLog);
            MakeColorOption("Success Log", ref settings.colors.successLog);
            GUILayout.EndVertical();
            GUILayout.Space(10);

        }

        static void MakeColorOption(string name, ref Color color)
        {
            int textureSize = 20;
            Texture texture = null;
            if (_textures.ContainsKey(name))
            {
                texture = _textures[name];
            }
            else
            {
                texture = CreateTexture.WithColor(color, textureSize);
                _textures.Add(name, texture);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(name, GUILayout.Width(100));
            GUILayout.Label(texture, GUILayout.Width(textureSize), GUILayout.Height(textureSize));
            if (UnityModManager.UI.DrawColor(ref color))
            {
                texture = CreateTexture.WithColor(color, textureSize);
            }
            GUILayout.EndHorizontal();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }
}
