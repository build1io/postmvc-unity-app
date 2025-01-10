using System;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
    public interface IInternetReachabilityController
    {
        bool LastResult     { get; }
        long LastResultCode { get; }
        long EmptyResultCode { get; }
        
        void Check(Action<bool> onComplete = null);
        void Check(int timeout, Action<bool> onComplete = null);
        void Check(string url, Action<bool> onComplete = null);
        void Check(string url, int timeout, Action<bool> onComplete = null);
    }
}