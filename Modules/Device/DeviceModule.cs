using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.UnityApp.Modules.Device.Impl;

namespace Build1.PostMVC.UnityApp.Modules.Device
{
    public sealed class DeviceModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            #if UNITY_EDITOR
                InjectionBinder.Bind<IDeviceController>().To<DeviceControllerEditor>().AsSingleton().ConstructOnStart();
            #else
                InjectionBinder.Bind<IDeviceController>().To<DeviceControllerRuntime>().AsSingleton().ConstructOnStart();
            #endif
        }
    }
}