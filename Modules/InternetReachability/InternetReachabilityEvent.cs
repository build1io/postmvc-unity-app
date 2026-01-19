using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
    public static class InternetReachabilityEvent
    {
        public static readonly Event<bool> CheckComplete = new(typeof(InternetReachabilityEvent), nameof(CheckComplete));
    }
}