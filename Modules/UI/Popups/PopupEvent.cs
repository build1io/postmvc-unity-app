using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.UI.Popups
{
    public static class PopupEvent
    {
        public static readonly Event<PopupBase> Open   = new(typeof(PopupEvent), nameof(Open));
        public static readonly Event<PopupBase> Closed = new(typeof(PopupEvent), nameof(Closed));
    }
}