using System.Collections.Generic;

namespace RSSFeeder.Models
{
    internal static class Settings
    {
        internal static List<string> UserURLs = new List<string>();
        internal static string DefaultUrl  = "";
        internal static uint UpdateTime = 60;

        public static void Load()
        {
            UserURLs = AppSettings.Default.UserURLs;
            DefaultUrl = AppSettings.Default.DefaultURL;
            UpdateTime = AppSettings.Default.UpdateTime;
        }

        public static void Save()
        {
            AppSettings.Default.UserURLs = UserURLs;
            AppSettings.Default.DefaultURL = DefaultUrl;
            AppSettings.Default.UpdateTime = UpdateTime;
            AppSettings.Default.Save();
        }
    }
}
