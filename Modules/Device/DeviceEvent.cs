using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.Device
{
    public static class DeviceEvent
    {
        public static readonly Event<DeviceType>              DeviceTypeChanged        = new Event<DeviceType>();
        public static readonly Event<DeviceOrientation>       DeviceOrientationChanged = new Event<DeviceOrientation>();
        public static readonly Event<DeviceScreenOrientation> ScreenOrientationChanged = new Event<DeviceScreenOrientation>();
    }
}