using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.App
{
    public abstract class AppEvent
    {
        public static readonly Event<bool> Pause = new Event<bool>();
        public static readonly Event<bool> Focus = new Event<bool>();

        public static readonly Event Restarting = new Event();
        public static readonly Event Quitting   = new Event();
    }
}