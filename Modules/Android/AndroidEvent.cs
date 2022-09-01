#if UNITY_ANDROID || UNITY_EDITOR

using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.Android
{
    public static class AndroidEvent
    {
        public static readonly Event Back = new();
    }
}

#endif