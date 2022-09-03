using System.Collections.Generic;

namespace Build1.PostMVC.Unity.App.Modules.Logging
{
    public interface ILogController
    {
        void RecordMessage(string message, bool forceFlush);

        string GetLog();
        void   FlushLog();

        List<LogFile> GetLogFiles();
        LogFile       GetLastLogFile();
        void          DeleteLogFiles();
    }
}