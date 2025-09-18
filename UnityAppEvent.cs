using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App
{
    public static class UnityAppEvent
    {
        public static readonly Event Start = new(typeof(UnityAppEvent), nameof(Start));
    }
}