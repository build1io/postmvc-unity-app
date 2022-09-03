using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Logging.Impl;

namespace Build1.PostMVC.Unity.App.Modules.Logging
{
    public sealed class LogModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<ILogController, LogController>().ConstructOnStart();
            InjectionBinder.Bind<ILog, LogProvider, LogAttribute>();
        }
    }
}