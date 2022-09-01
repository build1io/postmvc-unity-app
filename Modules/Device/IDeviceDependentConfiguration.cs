namespace Build1.PostMVC.UnityApp.Modules.Device
{
    public interface IDeviceDependentConfiguration
    {
        DevicePlatform DevicePlatform { get; }
        DeviceType     DeviceType     { get; }
    }
}