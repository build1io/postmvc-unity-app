using System;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
    public interface IInternetReachabilityController
    {
        bool LastResult     { get; }
        long LastResultCode { get; }

        void Check(Action<bool> onComplete, int timeout);
        void Check(Action<bool> onComplete);
    }
}