using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability.Commands
{
    public sealed class CheckInternetConnectionSilentCommand : Command
    {
        [Inject] private IInternetReachabilityController InternetReachabilityController { get; set; }

        public override void Execute()
        {
            InternetReachabilityController.Check();
        }
    }
}