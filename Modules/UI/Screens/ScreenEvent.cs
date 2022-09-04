using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.UI.Screens
{
    public static class ScreenEvent
    {
        public static readonly Event<ScreenBase> Created   = new();
        public static readonly Event<ScreenBase> Destroyed = new();

        public static readonly Event<ScreenBase> Shown  = new();
        public static readonly Event<ScreenBase> Hidden = new();
    }
}