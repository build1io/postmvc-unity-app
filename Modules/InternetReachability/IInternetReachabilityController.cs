using System;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
    public interface IInternetReachabilityController
    {
        bool LastResult     { get; }
        long LastResultCode { get; }
        long EmptyResultCode { get; }

        void SetCheckParams(string url, int timeout);
        
        void Check();
        void Check(Action<bool> onComplete);
        void Check(int timeout, Action<bool> onComplete);
    }
}