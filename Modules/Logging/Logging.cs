namespace Build1.PostMVC.Unity.App.Modules.Logging
{
    public static class Logging
    {
        public static LogLevel ForceLevel     = LogLevel.None;
        public static LogLevel MinLevel       = LogLevel.Debug;
        public static bool     Print          = true;
        public static bool     Record         = false;
        public static bool     SaveToFile     = false;
        public static byte     FlushThreshold = 128;
        public static byte     RecordsHistory = 10;
    }
}