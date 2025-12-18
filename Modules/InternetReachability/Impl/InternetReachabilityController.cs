using System;
using System.Collections;
using Build1.PostMVC.Core.MVCS.Events;
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
        [Inject]                public IEventDispatcher   Dispatcher        { get; set; }

        public bool  IsChecking     => _coroutine != null;
        public bool? LastResult     { get; private set; }
        public long  LastResultCode { get; private set; } = -1;

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

        public void Configure(string url, int timeout)
        {
            if (string.IsNullOrWhiteSpace(_url))
                throw new ArgumentException($"Url can't be null or empty. URL: {url}");

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
            if (IsChecking)
            {
                Log.Debug("Internet reachability check is in progress");
                return;
            }

            _coroutine = CoroutineProvider.StartCoroutine(CheckImpl(_url, _timeout));
        }

        /*
         * Private.
         */

        private IEnumerator CheckImpl(string url, int timeout)
        {
            Log.Debug((u, t) => $"Checking internet... URL: {u} Timeout: {t}", url, timeout);

            bool result;
            long code;

            using (var request = UnityWebRequest.Head(url))
            {
                request.timeout = timeout;

                yield return request.SendWebRequest();

                // Internet is reachable if result is Success, ProtocolError, or event DataProcessingError is returned.
                // The mean internet is reachable even if server return an error.
                result = request.result != UnityWebRequest.Result.ConnectionError;
                code = request.responseCode;
            }

            Log?.Debug((r, c) => $"Internet check complete. Result: {r} Code: {c}", result, code);

            LastResult = result;
            LastResultCode = code;
            
            _coroutine = null;

            Dispatcher?.Dispatch(InternetReachabilityEvent.CheckComplete, result, code);
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}