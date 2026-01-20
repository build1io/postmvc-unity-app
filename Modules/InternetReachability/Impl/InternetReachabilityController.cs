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

        private const string URL     = "https://clients3.google.com/generate_204";
        private const int    Timeout = 3;

        private Coroutine _coroutine;

        [PreDestroy]
        public void PreDestroy()
        {
            CoroutineProvider.StopCoroutine(ref _coroutine);
        }

        /*
         * Public.
         */

        public void Check()
        {
            if (IsChecking)
            {
                Log.Debug("Internet reachability check is in progress");
                return;
            }

            _coroutine = CoroutineProvider.StartCoroutine(CheckImpl(result =>
            {
                Log?.Debug(r => $"Internet check complete. Result: {r}", result);

                LastResult = result;

                _coroutine = null;

                Dispatcher?.Dispatch(InternetReachabilityEvent.CheckComplete, result);
            }));
        }

        /*
         * Private.
         */

        private IEnumerator CheckImpl(Action<bool> onComplete)
        {
            var url = $"{URL}?t={DateTime.Now.Ticks}";
            var timeout = Timeout;
            
            Log.Debug((u, t) => $"Checking internet... URL: {u} Timeout: {t}", url, timeout);

            using var request = UnityWebRequest.Head(url);
            
            request.timeout = timeout;

            yield return request.SendWebRequest();

            Log.Debug(r => $"Result: {r.result} Code: {r.responseCode}", request);

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                // Note: ProtocolError can happen on Captive Portals (redirects), 
                // but we check the ResponseCode specifically below to be sure.
                
                // If the error is purely network-related (DNS fail, timeout):
                if (request.responseCode == 0) 
                {
                    onComplete(false);
                    yield break;
                }
            }
                
            var responseCode = request.responseCode;
            switch (responseCode)
            {
                case 204:
                    onComplete(true);
                    break;
                default:
                    onComplete(false);
                    break;
            }
        }
    }
}