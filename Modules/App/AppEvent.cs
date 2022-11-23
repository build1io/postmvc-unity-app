using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.App
{
    public abstract class AppEvent
    {
        public static readonly Event<bool> Pause      = new(typeof(AppEvent), nameof(Pause));
        public static readonly Event<bool> Focus      = new(typeof(AppEvent), nameof(Focus));
        public static readonly Event       Restarting = new(typeof(AppEvent), nameof(Restarting));
        public static readonly Event       Quitting   = new(typeof(AppEvent), nameof(Quitting));
    }
}