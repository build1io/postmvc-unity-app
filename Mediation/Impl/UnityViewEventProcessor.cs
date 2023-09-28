using System;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.App.Mediation.Impl
{
    internal sealed class UnityViewEventProcessor
    {
        [Inject] public IInjectionReflector InjectionReflector { get; set; }
        
        public void ProcessStart(IUnityView view)
        {
            ProcessInstanceMethods<Start>(view);
            ProcessInstanceMethods<Start>(view.Mediator);
        }

        public void ProcessOnEnable(IUnityView view)
        {
            ProcessInstanceMethods<OnEnable>(view);
            ProcessInstanceMethods<OnEnable>(view.Mediator);
        }

        public void ProcessOnDisable(IUnityView view)
        {
            ProcessInstanceMethods<OnDisable>(view);
            ProcessInstanceMethods<OnDisable>(view.Mediator);
        }
        
        /*
         * Private.
         */

        private void ProcessInstanceMethods<T>(object instance) where T : Attribute
        {
            if (instance == null)
                return;
            
            var info = InjectionReflector.Get(instance.GetType());
            var methods = info.GetMethodInfos<T>();
            if (methods == null)
                return;
            
            foreach (var method in methods)
                method.Invoke(instance, null);
        }
    }
}