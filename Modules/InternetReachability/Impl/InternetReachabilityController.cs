using System;
using System.Collections;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Coroutines;
using Build1.PostMVC.Unity.App.Modules.Logging;
using UnityEngine;
using UnityEngine.Networking;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability.Impl
{
    internal sealed class InternetReachabilityController : IInternetReachabilityController
    {
        [Log(LogLevel.Warning)] public ILog               Log               { get; set; }
        [Inject]                public ICoroutineProvider CoroutineProvider { get; set; }

        public bool LastResult      { get; private set; }
        public long LastResultCode  { get; private set; } = -1;
        public long EmptyResultCode { get; }              = -1;

        private string    _url     = "https://google.com";
        private int       _timeout = 3;
        private Coroutine _coroutine;

        [PreDestroy]
        public void PreDestroy()
        {
            CoroutineProvider.StopCoroutine(ref _coroutine);
        }

        /*
         * Public.
         */

        public void SetCheckParams(string url = "https://google.com", int timeout = 3)
        {
            if (!IsValidUrl(url))
                throw new ArgumentException($"Url is invalid. URL: {url}");

            if (timeout <= 0)
                throw new ArgumentException($"Invalid timeout. Timeout: {timeout}");

            _url = url;
            _timeout = timeout;

            Log.Debug((u, t) => $"Internet check params set. URL: {u} Timeout: {t}", _url, _timeout);
        }

        public void Check()
        {
            if (string.IsNullOrWhiteSpace(_url))
                throw new ArgumentException("Internet reachability url is not set");

            _coroutine = CoroutineProvider.StartCoroutine(CheckImpl(_url, _timeout, null));
        }

        public void Check(Action<bool> onComplete)
        {
            if (string.IsNullOrWhiteSpace(_url))
                throw new ArgumentException("Internet reachability url is not set");

            _coroutine = CoroutineProvider.StartCoroutine(CheckImpl(_url, _timeout, onComplete));
        }

        public void Check(int timeout, Action<bool> onComplete)
        {
            if (string.IsNullOrWhiteSpace(_url))
                throw new ArgumentException("Internet reachability url is not set");

            _coroutine = CoroutineProvider.StartCoroutine(CheckImpl(_url, timeout, onComplete));
        }

        /*
         * Private.
         */

        private IEnumerator CheckImpl(string url, int timeout, Action<bool> onComplete)
        {
            Log.Debug((u, t) => $"Checking internet... URL: {u} Timeout: {t}", url, timeout);

            bool result;
            using (var request = UnityWebRequest.Head(url))
            {
                request.timeout = timeout;
                yield return request.SendWebRequest();
                result = request.responseCode == 200;

                LastResult = result;
                LastResultCode = request.responseCode;
            }

            if (CoroutineProvider == null)
                yield break;

            Log.Debug(r => $"Internet availability: {r}", result);

            onComplete?.Invoke(result);
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}