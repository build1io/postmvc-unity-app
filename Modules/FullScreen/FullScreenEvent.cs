using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.FullScreen
{
    public static class FullScreenEvent
    {
        public static readonly Event<bool> Changed = new();
    }
}