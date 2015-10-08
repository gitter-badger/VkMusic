using System;
using System.IO;

namespace VkMusicSync
{
    public static class AppPaths
    {
        // TODO do something with this class, contains globals for two assemblies
        public static string AppPath 
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

        public static string SettingsPath 
            => Path.Combine(AppPath, @"Settings\");

        public static string WebSessionPath 
            => Path.Combine(AppPath, @"WebSession\");

        public static string AppName 
            => "VkMusicSync";
    }
}
