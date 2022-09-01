using System;

namespace Build1.PostMVC.UnityApp.Modules.Logging
{
    public interface ILogWarn
    {
        void Warn(string message);
        void Warn(Exception exception);
    }
}