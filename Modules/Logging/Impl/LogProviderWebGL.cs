#if UNITY_WEBGL

using System;
using System.Runtime.InteropServices;
using System.Web;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.App.Modules.Logging.Impl
{
    internal sealed class LogProviderWebGL : LogProviderBase
    {
        [DllImport("__Internal")]
        private static extern void LogDebug(string message);

        [DllImport("__Internal")]
        private static extern void LogError(string message);
        
        [DllImport("__Internal")]
        private static extern string GetUrlParameters();
        
        [PostConstruct]
        public void PostConstruct()
        {
            var urlParams = HttpUtility.ParseQueryString(GetUrlParameters().ToLower());

            try
            {
                var logLevelString = urlParams["loglevel"];
                if (string.IsNullOrWhiteSpace(logLevelString))
                    return;
                
                var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelString, true);
                if (Enum.IsDefined(typeof(LogLevel), logLevel))
                {
                    // Overriding global setting if another log level is set via query string params.
                    Logging.Print = logLevel > LogLevel.None;
                    Logging.PrintLevel = logLevel;

                    LogDebug($"{nameof(LogWebGL)}: Global log level overridden to {logLevel}\n");    
                }
            }
            catch (Exception exception)
            {
                LogError(LogBase.FormatException(nameof(LogWebGL), exception));
            }
        }
        
        /*
         * Public.
         */
        
        public override ILog CreateLogInstance(string prefix, LogLevel level, ILogController logController)
        {
            return new LogWebGL(prefix, level, logController);
        }
    }
}

#endif