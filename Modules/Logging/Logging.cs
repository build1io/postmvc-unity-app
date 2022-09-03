using System;
using Build1.PostMVC.Unity.App.Modules.Logging.Impl;
using UnityEngine;

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
        
        public static ILog GetLog<T>(LogLevel level)
        {
            var log = GetLogImpl(typeof(T), level, Core.PostMVC.GetInstance<ILogController>());

            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                log.Warn("You're getting a logger during MonoBehavior instantiation. " +
                         "This may end up in script instantiation exception on a device. " +
                         "Consider inheriting of component from UnityView and injecting a logger.");
            }

            return log;
        }

        public static ILog GetLog(object owner, LogLevel level)
        {
            return GetLogImpl(owner.GetType(), level, Core.PostMVC.GetInstance<ILogController>());
        }

        internal static ILog GetLogImpl(Type ownerType, LogLevel level, ILogController logController)
        {
            if (Print || Record)
            {
                if (ForceLevel != LogLevel.None) // Using force level if it was set.
                    level = ForceLevel;
                else if (level > LogLevel.None && level < MinLevel) // Filtering logs by min log level (could be set different for production environment).
                    level = MinLevel;

                if (level != LogLevel.None)
                {
                    #if UNITY_WEBGL && !UNITY_EDITOR
                
                    return new LogWebGL(ownerType.Name, level, logController);

                    #else

                    return new LogDefault(ownerType.Name, level, logController);

                    #endif
                }    
            }
            
            return new LogVoid();
        }
    }
}