using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Modules.Device
{
    public static class DeviceEvent
    {
        public static readonly Event<DeviceType>              DeviceTypeChanged        = new(typeof(DeviceEvent), nameof(DeviceTypeChanged));
        public static readonly Event<DeviceOrientation>       DeviceOrientationChanged = new(typeof(DeviceEvent), nameof(DeviceOrientationChanged));
        public static readonly Event<DeviceScreenOrientation> ScreenOrientationChanged = new(typeof(DeviceEvent), nameof(ScreenOrientationChanged));
    }
}