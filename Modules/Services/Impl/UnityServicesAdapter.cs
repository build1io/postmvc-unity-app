using System;
using System.Threading.Tasks;
using Build1.PostMVC.Unity.App.Modules.Async;
using Unity.Services.Core;

namespace Build1.PostMVC.Unity.App.Modules.Services.Impl
{
    public sealed class UnityServicesAdapter
    {
        public static bool Initialized { get; private set; }

        public static event Action            OnInitialized;
        public static event Action<Exception> OnError;

        public static void Initialize()
        {
            UnityServices.InitializeAsync().Resolve(OnInitializedHandler);
        }

        private static void OnInitializedHandler(Task task)
        {
            if (task.IsCanceled)
            {
                OnError?.Invoke(new Exception("Game Services async initialization cancelled."));
                return;
            }

            if (task.Exception != null)
            {
                OnError?.Invoke(task.Exception);
                return;
            }

            Initialized = true;
            OnInitialized?.Invoke();
        }
    }
}