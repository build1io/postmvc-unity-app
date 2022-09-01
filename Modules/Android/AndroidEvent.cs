#if UNITY_ANDROID || UNITY_EDITOR

using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.Android
{
    public static class AndroidEvent
    {
        public static readonly Event Back = new();
    }
}

#endif