using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.Popups
{
    public static class PopupEvent
    {
        public static readonly Event<PopupBase> Open   = new Event<PopupBase>();
        public static readonly Event<PopupBase> Closed = new Event<PopupBase>();
    }
}