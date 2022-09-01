using System;
using System.Collections;
using System.IO;
using Build1.PostMVC.Unity.App.Modules.Assets.Impl.Cache;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;

namespace Build1.PostMVC.Unity.App.Modules.Assets.Impl.Agents
{
    internal abstract class AssetsAgentBase : MonoBehaviour
    {
        public event Func<string, SpriteAtlas> AtlasRequested;

        #if UNITY_EDITOR

        private bool _safeMode;

        #endif

        /*
         * Public.
         */

        public abstract void LoadAsync(AssetBundleInfo info,
                                       Func<AssetBundleInfo, AssetBundleCacheInfo> onCacheInfoGet,
                                       Action<AssetBundleInfo> onCacheInfoClean,
                                       Action<string, AssetBundleInfo> onCacheInfoRecord,
                                       Action<AssetBundleInfo, float, ulong> onProgress,
                                       Action<AssetBundleInfo, AssetBundle> onComplete,
                                       Action<AssetBundleInfo, AssetsException> onError);

        /*
         * Protected.
         */

        protected IEnumerator LoadEmbedAssetBundleCoroutine(AssetBundleInfo info,
                                                            Action<AssetBundleInfo, float, ulong> onProgress,
                                                            Action<AssetBundleInfo, AssetBundle> onComplete,
                                                            Action<AssetBundleInfo, AssetsException> onError)
        {
            var path = Path.Combine(Application.streamingAssetsPath, info.BundleId);
            var request = AssetBundle.LoadFromFileAsync(path);

            while (!request.isDone && !info.IsAborted)
            {
                onProgress.Invoke(info, request.progress, 0);
                yield return null;
            }

            if (info.IsAborted)
            {
                onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingAborted, path));
            }
            else if (request.assetBundle == null)
            {
                onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleNotFound, path));
            }
            else
            {
                onProgress.Invoke(info, request.progress, 0);
                onComplete.Invoke(info, request.assetBundle);
            }
        }

        protected IEnumerator LoadRemoteAssetBundleCoroutine(AssetBundleInfo info,
                                                             Func<AssetBundleInfo, AssetBundleCacheInfo> onCacheInfoGet,
                                                             Action<AssetBundleInfo> onCacheInfoClean,
                                                             Action<string, AssetBundleInfo> onCacheInfoRecord,
                                                             Action<AssetBundleInfo, float, ulong> onProgress,
                                                             Action<AssetBundleInfo, AssetBundle> onComplete,
                                                             Action<AssetBundleInfo, AssetsException> onError)
        {
            if (info.IsCacheEnabled && !string.IsNullOrWhiteSpace(info.CacheId) && onCacheInfoGet != null && onCacheInfoClean != null)
            {
                var cacheInfo = onCacheInfoGet.Invoke(info);
                if (cacheInfo != null)
                {
                    // If URL or version is different, we unload cache and info.
                    if (cacheInfo.BundleUrl != info.BundleUrl || cacheInfo.BundleVersion != info.BundleVersion)
                    {
                        Caching.ClearAllCachedVersions(cacheInfo.BundleName);
                        
                        onCacheInfoClean.Invoke(info);
                    }
                }
            }
            
            var request = info.IsCacheEnabled
                              ? UnityWebRequestAssetBundle.GetAssetBundle(info.BundleUrl, info.BundleVersion, 0)
                              : UnityWebRequestAssetBundle.GetAssetBundle(info.BundleUrl);

            request.SendWebRequest();

            while (!request.isDone)
            {
                onProgress.Invoke(info, request.downloadProgress, request.downloadedBytes);

                if (info.IsAborted)
                    request.Abort();

                yield return null;
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                onProgress.Invoke(info, request.downloadProgress, request.downloadedBytes);

                var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                
                if (info.IsCacheEnabled && !string.IsNullOrWhiteSpace(info.CacheId) && onCacheInfoRecord != null)
                    onCacheInfoRecord.Invoke(assetBundle.name, info);    
                
                onComplete.Invoke(info, assetBundle);
                yield break;
            }

            var isAndroidStorageError = request.error.Contains("Unable to write data");
            var isIOSStorageError = request.error.Contains("Data Processing Error, see Download Handler error") &&
                                    request.downloadHandler.error.Contains("Failed to decompress data for the AssetBundle");

            if (isAndroidStorageError || isIOSStorageError)
            {
                onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingStorageError, request.downloadHandler.error));
                yield break;
            }

            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingNetworkError, request.error));
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingHttpError, request.error));
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingProcessingError, request.error));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /*
         * Unity Events.
         */

        public void OnEnable()
        {
            #if UNITY_EDITOR

            StartCoroutine(HandleSafeModeForEditor());

            #endif

            SpriteAtlasManager.atlasRequested += RequestAtlas;
        }

        public void OnDisable()
        {
            #if UNITY_EDITOR

            StopAllCoroutines();

            #endif

            SpriteAtlasManager.atlasRequested -= RequestAtlas;
        }

        #if UNITY_EDITOR

        IEnumerator HandleSafeModeForEditor()
        {
            _safeMode = true;

            yield return 0;

            _safeMode = false;
        }

        #endif

        /*
         * Event Handlers.
         */

        private void RequestAtlas(string atlasId, Action<SpriteAtlas> onComplete)
        {
            #if UNITY_EDITOR

            if (_safeMode)
            {
                try
                {
                    onComplete(AtlasRequested?.Invoke(atlasId));
                }
                catch
                {
                    onComplete(null);
                }

                return;
            }

            #endif

            onComplete(AtlasRequested?.Invoke(atlasId));
        }
    }
}