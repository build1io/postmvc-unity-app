using System;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
    public interface IInternetReachabilityController
    {
        bool  IsChecking { get; }
        bool? LastResult { get; }

        void Check();
        void FlushLogs(Action<string> handler);
    }
}