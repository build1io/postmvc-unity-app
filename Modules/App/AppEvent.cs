using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.App
{
    public abstract class AppEvent
    {
        public static readonly Event<bool> Pause = new();
        public static readonly Event<bool> Focus = new();

        public static readonly Event Restarting = new();
        public static readonly Event Quitting   = new();
    }
}