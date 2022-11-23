using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.UI.Screens
{
    public static class ScreenEvent
    {
        public static readonly Event<ScreenBase> Created   = new(typeof(ScreenEvent), nameof(Created));
        public static readonly Event<ScreenBase> Destroyed = new(typeof(ScreenEvent), nameof(Destroyed));
        public static readonly Event<ScreenBase> Shown     = new(typeof(ScreenEvent), nameof(Shown));
        public static readonly Event<ScreenBase> Hidden    = new(typeof(ScreenEvent), nameof(Hidden));
    }
}