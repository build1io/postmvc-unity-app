using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.Logging
{
    public static class LogEvent
    {
        public static readonly Event<string> Flush = new(typeof(LogEvent), nameof(Flush));
    }
}