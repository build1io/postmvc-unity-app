using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability.Commands
{
    public sealed class CheckInternetConnectionCommand : Command
    {
        [Inject] public  IEventDispatcher                Dispatcher                     { get; set; }
        [Inject] private IInternetReachabilityController InternetReachabilityController { get; set; }

        public override void Execute()
        {
            Retain();

            if (InternetReachabilityController.LastResult.HasValue && InternetReachabilityController.LastResult.Value)
            {
                OnInternetCheckComplete(InternetReachabilityController.LastResult.Value, InternetReachabilityController.LastResultCode);
                return;
            }

            Dispatcher.AddListenerOnce(InternetReachabilityEvent.CheckComplete, OnInternetCheckComplete);

            if (!InternetReachabilityController.IsChecking)
                InternetReachabilityController.Check();
        }

        private void OnInternetCheckComplete(bool reachable, long requestCode)
        {
            if (reachable)
                Release();
            else
                Fail(new NoInternetException());
        }
    }
}