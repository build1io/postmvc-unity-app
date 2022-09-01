using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.UnityApp.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.UnityApp.Modules.FullScreen.Impl
{
    internal sealed class FullScreenController : IFullScreenController
    {
        [Log(LogLevel.Warning)] public ILog             Log        { get; set; }
        [Inject]                public IEventDispatcher Dispatcher { get; set; }

        public bool IsInFullScreen { get; private set; }

        public void ToggleFullScreen()
        {
            if (Application.isEditor)
            {
                Log.Warn("Full screen is not supported in Editor.");
                return;
            }

            // Fullscreen will be out of sync if a player exits fullscreen by himself.

            Screen.fullScreen = !Screen.fullScreen;
            IsInFullScreen = Screen.fullScreen;

            Dispatcher.Dispatch(FullScreenEvent.Changed, IsInFullScreen);
        }
    }
}