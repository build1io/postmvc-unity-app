using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.UnityApp.Modules.InternetReachability.Impl;

namespace Build1.PostMVC.UnityApp.Modules.InternetReachability
{
    internal sealed class InternetReachabilityModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IInternetReachabilityController>().To<InternetReachabilityController>().AsSingleton();
        }
    }
}