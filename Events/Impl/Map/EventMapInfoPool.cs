using Build1.PostMVC.Core.MVCS.Events.Impl.Map;
using Build1.PostMVC.Core.Utils.Pooling;

namespace Build1.PostMVC.Unity.App.Events.Impl.Map
{
    internal partial class EventMapInfoPool : Pool<IEventMapInfo>
    {
        public EventMapInfoUnityViewDispatcher             TakeUnityViewDispatcher()             { return Take<EventMapInfoUnityViewDispatcher>(); }
        public EventMapInfoUnityViewDispatcher<T1>         TakeUnityViewDispatcher<T1>()         { return Take<EventMapInfoUnityViewDispatcher<T1>>(); }
        public EventMapInfoUnityViewDispatcher<T1, T2>     TakeUnityViewDispatcher<T1, T2>()     { return Take<EventMapInfoUnityViewDispatcher<T1, T2>>(); }
        public EventMapInfoUnityViewDispatcher<T1, T2, T3> TakeUnityViewDispatcher<T1, T2, T3>() { return Take<EventMapInfoUnityViewDispatcher<T1, T2, T3>>(); }
    }
}