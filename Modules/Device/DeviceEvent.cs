using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.Device
{
    public static class DeviceEvent
    {
        public static readonly Event<DeviceType>              DeviceTypeChanged        = new();
        public static readonly Event<DeviceOrientation>       DeviceOrientationChanged = new();
        public static readonly Event<DeviceScreenOrientation> ScreenOrientationChanged = new();
    }
}