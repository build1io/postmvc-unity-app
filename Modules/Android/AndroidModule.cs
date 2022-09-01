#if UNITY_ANDROID || UNITY_EDITOR

using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.UnityApp.Modules.Agents;
using Build1.PostMVC.UnityApp.Modules.Android.Impl;

namespace Build1.PostMVC.UnityApp.Modules.Android
{
    public sealed class AndroidModule : Module
    {
        [Inject] public IAgentsController AgentsController { get; set; }
        [Inject] public IEventDispatcher  Dispatcher       { get; set; }

        private AndroidAgent _androidAgent;

        [PostConstruct]
        public void PostConstruct()
        {
            _androidAgent = AgentsController.Create<AndroidAgent>();
            _androidAgent.OnBack += OnBack;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            _androidAgent.OnBack -= OnBack;
            AgentsController.Destroy(ref _androidAgent);
        }

        /*
         * Event Handlers.
         */

        private void OnBack()
        {
            Dispatcher.Dispatch(AndroidEvent.Back);
        }
    }
}

#endif