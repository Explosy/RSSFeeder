using System.Collections.Generic;

namespace RSSFeeder.Models
{
    internal static class Settings
    {
        internal static List<string> UserUrls { get; set; } = new List<string>();
        internal static string DefaultUrl { get; set; } = "";
        internal static uint UpdateTime { get; set; }


        public static void Load()
        {
            UserUrls = AppSettings.Default.UserURLs;
            DefaultUrl = AppSettings.Default.DefaultURL;
            UpdateTime = AppSettings.Default.UpdateTime;
        }

        public static void Save()
        {
            AppSettings.Default.UserURLs = UserUrls;
            AppSettings.Default.DefaultURL = DefaultUrl;
            AppSettings.Default.UpdateTime = UpdateTime;
            AppSettings.Default.Save();

        }
    }
}
