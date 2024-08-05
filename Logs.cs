using System;
using System.Text;
using UnityEngine;

namespace BetterUnityModManagerLogger
{
    public static class Logs
    {
        public static string GetCurrentTimePrefix()
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss")}]";
        }

        public static string AddColorXML(string str, string colorXml)
        {
            const string XML_COLOR_START = "<color=";
            const string XML_COLOR_END = "</color>";

            StringBuilder stringBuilder = new StringBuilder(XML_COLOR_START)
                .Append(colorXml)
                .Append('>')
                .Append(str)
                .Append(XML_COLOR_END);
            str = stringBuilder.ToString();
            return str;
        }

        public static bool IsError(string str)
        {
            str = str.ToLower();
            return str.Contains("[error]") || str.Contains("[critical]") || str.Contains("[exception]");
        }
        public static bool IsWarning(string str)
        {
            str = str.ToLower();
            return str.Contains("[warning]");
        }

        public static string GetColorHTML(string str)
        {
            Settings.Colors settings = Main.settings.colors;

            str = str.ToLower();
            if (str.Contains("[error]"))
                return ColorUtility.ToHtmlStringRGBA(settings.errorLog);
            if (str.Contains("[critical]"))
                return ColorUtility.ToHtmlStringRGBA(settings.criticalLog);
            if (str.Contains("[exception]"))
                return ColorUtility.ToHtmlStringRGBA(settings.exceptionLog);
            if (str.Contains("[warning]"))
                return ColorUtility.ToHtmlStringRGBA(settings.warningLog);
            if (str.Contains("[success]"))
                return ColorUtility.ToHtmlStringRGBA(settings.successLog);

            return ColorUtility.ToHtmlStringRGBA(settings.defaultLog);
        }
    }
}
