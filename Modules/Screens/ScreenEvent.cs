using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.Screens
{
    public static class ScreenEvent
    {
        public static readonly Event<Screen> Created   = new Event<Screen>();
        public static readonly Event<Screen> Destroyed = new Event<Screen>();

        public static readonly Event<Screen> Shown  = new Event<Screen>();
        public static readonly Event<Screen> Hidden = new Event<Screen>();
    }
}