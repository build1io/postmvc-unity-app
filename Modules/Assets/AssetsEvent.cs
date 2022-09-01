using System;
using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.UnityApp.Modules.Assets
{
    public static class AssetsEvent
    {
        public static readonly Event<AssetBundleInfo>            BundleLoadingProgress = new Event<AssetBundleInfo>();
        public static readonly Event<AssetBundleInfo>            BundleLoadingSuccess  = new Event<AssetBundleInfo>();
        public static readonly Event<AssetBundleInfo, Exception> BundleLoadingFail     = new Event<AssetBundleInfo, Exception>();
    }
}