using System;

namespace Build1.PostMVC.UnityApp.Modules.Logging
{
    public interface ILogDebug
    {
        void Debug(string message);
        void Debug(Exception exception);
    }
}