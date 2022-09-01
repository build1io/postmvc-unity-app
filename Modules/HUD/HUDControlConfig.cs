using System;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.Assets;
using Build1.PostMVC.Unity.App.Modules.Device;
using Build1.PostMVC.Unity.App.Modules.UI;

namespace Build1.PostMVC.Unity.App.Modules.HUD
{
    public sealed class HUDControlConfig : UIControlConfiguration
    {
        public HUDControlConfig(AssetBundleInfo bundleInfo, 
                                string prefabName, 
                                int appLayerId) : base(bundleInfo, prefabName, appLayerId) { }
        
        public HUDControlConfig(Enum bundleId, 
                                string prefabName, 
                                int appLayerId) : base(bundleId, prefabName, appLayerId) { }
        
        public HUDControlConfig(DevicePlatform platform, 
                                Enum bundleId, 
                                string prefabName, 
                                int appLayerId) : base(platform, bundleId, prefabName, appLayerId) { }
        
        public HUDControlConfig(DeviceType deviceType, 
                                Enum bundleId, 
                                string prefabName, 
                                int appLayerId) : base(deviceType, bundleId, prefabName, appLayerId) { }
        
        public HUDControlConfig(DevicePlatform platform, 
                                DeviceType deviceType, 
                                Enum bundleId, 
                                string prefabName, 
                                int appLayerId) : base(platform, deviceType, bundleId, prefabName, appLayerId) { }

        public new HUDControlConfig AddBinding<V, M>() where V : IUnityView
                                                       where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new HUDControlConfig AddBinding<V, I, M>() where V : IUnityView, I
                                                          where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}