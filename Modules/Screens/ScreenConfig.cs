using System;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.Assets;
using Build1.PostMVC.Unity.App.Modules.Device;
using Build1.PostMVC.Unity.App.Modules.UI;

namespace Build1.PostMVC.Unity.App.Modules.Screens
{
    public sealed class ScreenConfig : UIControlConfiguration
    {
        public ScreenConfig(string asset, int layerId) : base(asset, layerId) { }
        public ScreenConfig(string asset, int layerId, Enum assetBundleId) : base(asset, layerId, assetBundleId) { }
        
        public ScreenConfig(AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(bundleInfo, prefabName, appLayerId) { }
        public ScreenConfig(Enum bundleId, string prefabName, int appLayerId) : base(bundleId, prefabName, appLayerId) { }

        public ScreenConfig(DevicePlatform platform, AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(platform, bundleInfo, prefabName, appLayerId) { }
        public ScreenConfig(DevicePlatform platform, Enum bundleId, string prefabName, int appLayerId) : base(platform, bundleId, prefabName, appLayerId) { }

        public ScreenConfig(DeviceType deviceType, AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(deviceType, bundleInfo, prefabName, appLayerId) { }
        public ScreenConfig(DeviceType deviceType, Enum bundleId, string prefabName, int appLayerId) : base(deviceType, bundleId, prefabName, appLayerId) { }

        public ScreenConfig(DevicePlatform platform, DeviceType deviceType, AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(platform, deviceType, bundleInfo, prefabName, appLayerId) { }
        public ScreenConfig(DevicePlatform platform, DeviceType deviceType, Enum bundleId, string prefabName, int appLayerId) : base(platform, deviceType, bundleId, prefabName, appLayerId) { }

        public new ScreenConfig AddBinding<V, M>() where V : IUnityView
                                                   where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new ScreenConfig AddBinding<V, I, M>() where V : IUnityView, I
                                                      where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}