using System;
using Build1.PostMVC.Core.Extensions;
using Build1.PostMVC.Core.Extensions.MVCS;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.UnityApp.Contexts;
using Build1.PostMVC.UnityApp.Contexts.Impl;
using Build1.PostMVC.UnityApp.Events.Impl;
using Build1.PostMVC.UnityApp.Mediation.Api;
using Build1.PostMVC.UnityApp.Mediation.Impl;
using Build1.PostMVC.UnityApp.Modules.Agents;
using Build1.PostMVC.UnityApp.Modules.Agents.Impl;
using Build1.PostMVC.UnityApp.Modules.App;
using Build1.PostMVC.UnityApp.Modules.App.Impl;
using Build1.PostMVC.UnityApp.Modules.Assets;
using Build1.PostMVC.UnityApp.Modules.Assets.Impl;
using Build1.PostMVC.UnityApp.Modules.Async;
using Build1.PostMVC.UnityApp.Modules.Async.Impl;
using Build1.PostMVC.UnityApp.Modules.Coroutines;
using Build1.PostMVC.UnityApp.Modules.Coroutines.Impl;
using Build1.PostMVC.UnityApp.Modules.Device;
using Build1.PostMVC.UnityApp.Modules.FullScreen;
using Build1.PostMVC.UnityApp.Modules.InternetReachability;
using Build1.PostMVC.UnityApp.Modules.Logging;
using Build1.PostMVC.UnityApp.Modules.Popups;
using Build1.PostMVC.UnityApp.Modules.Popups.Impl;
using Build1.PostMVC.UnityApp.Modules.Screens;
using Build1.PostMVC.UnityApp.Modules.Screens.Impl;
using Build1.PostMVC.UnityApp.Modules.UI;
using Build1.PostMVC.UnityApp.Modules.UI.Impl;
using Build1.PostMVC.UnityApp.Modules.Update;
using Build1.PostMVC.UnityApp.Modules.Update.Impl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.UnityApp
{
    public sealed class UnityAppExtension : Extension
    {
        public const string RootGameObjectName = "[PostMVC]";

        private GameObject _contextViewGameObject;
        
        /*
         * Public.
         */

        public override void Initialize()
        {
            var injectionBinder = GetDependentExtension<MVCSExtension>().InjectionBinder;
            injectionBinder.Bind<ILog, LogProvider, LogAttribute>();
            injectionBinder.Rebind<IEventBus, EventBusUnity>();
            injectionBinder.Bind<IUnityViewEventProcessor, UnityViewEventProcessor>();
            injectionBinder.Bind<IAgentsController, AgentsController>();
            injectionBinder.Bind<IAppController, AppController>().ConstructOnStart();
            injectionBinder.Bind<IAssetsController, AssetsController>();
            injectionBinder.Bind<IAsyncResolver, AsyncResolver>().ConstructOnStart();
            injectionBinder.Bind<ICoroutineProvider, CoroutineProvider>();
            injectionBinder.Bind<IUpdateController, UpdateController>();

            // UI            
            injectionBinder.Bind<IPopupController, PopupController>().AsSingleton();
            injectionBinder.Bind<IScreensController, ScreensController>().AsSingleton();
            injectionBinder.Bind<IUILayersController, UILayersController>().AsSingleton();
            
            #if UNITY_ANDROID || UNITY_EDITOR
                Context.AddModule<Modules.Android.AndroidModule>();
            #endif
            
            Context.AddModule<DeviceModule>();
            Context.AddModule<FullScreenModule>();
            Context.AddModule<InternetReachabilityModule>();
        }

        public override void Setup()
        {
            var viewGameObject = GetViewGameObject(null);
            var viewAgent = viewGameObject.AddComponent<ContextViewUnity>();
            viewAgent.SetContext(Context);

            Object.DontDestroyOnLoad(viewGameObject);

            var injectionBinder = GetDependentExtension<MVCSExtension>().InjectionBinder;
            injectionBinder.Rebind<IContextView>(viewAgent);

            if (Context.IsRootContext)
            {
                viewGameObject.name = RootGameObjectName;
            }
            else
            {
                viewGameObject.name = GetType().Name;

                if (RootContext.HasExtension<MVCSExtension>())
                {
                    var rootInjectionBinder = RootContext.GetExtension<MVCSExtension>().InjectionBinder;
                    if (rootInjectionBinder.GetBinding<IContextView>() != null)
                    {
                        var rootView = rootInjectionBinder.GetInstance<IContextView>();
                        var rootGameObject = rootView.As<GameObject>();
                        viewGameObject.transform.SetParent(rootGameObject.transform);
                    }
                }
            }

            _contextViewGameObject = viewGameObject;
        }

        public override void Dispose()
        {
            // We don't need to unbind anything. MVCSExtension does it.
            // But we need to remove Context View Game Object.
            Object.Destroy(_contextViewGameObject);
            _contextViewGameObject = null;
        }

        /*
         * Private.
         */

        private static GameObject GetViewGameObject(object view)
        {
            switch (view)
            {
                case null:                    return new GameObject();
                case GameObject gameObject:   return gameObject;
                case MonoBehaviour behaviour: return behaviour.gameObject;
                default:
                    throw new Exception($"Specified view is not a GameObject or MonoBehavior. [{view.GetType()}]");
            }
        }
    }
}