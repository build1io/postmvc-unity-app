using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.Screens
{
    public static class ScreenEvent
    {
        public static readonly Event<Screen> Created   = new();
        public static readonly Event<Screen> Destroyed = new();

        public static readonly Event<Screen> Shown  = new();
        public static readonly Event<Screen> Hidden = new();
    }
}