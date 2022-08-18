using System;
using System.Collections.Generic;

namespace RSSFeeder.Models
{
    public static class Settings
    {
        public static List<string> UserURLs = new List<string>();
        public static string DefaultUrl  = "";
        public static uint UpdateTime = 60;

        public static void Load()
        {
            if (AppSettings.Default.UserURLs != null) UserURLs = AppSettings.Default.UserURLs;
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
