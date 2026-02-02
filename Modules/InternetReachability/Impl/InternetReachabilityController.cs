using System;
using System.Collections;
using System.Collections.Generic;
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

        public bool  IsChecking => _coroutine != null;
        public bool? LastResult { get; private set; }

        private const int Timeout = 3;

        private          Coroutine    _coroutine;
        private readonly List<string> _logs = new();

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

        public void FlushLogs(Action<string> handler)
        {
            foreach (var log in _logs)
            {
                Log.Debug(log);

                handler.Invoke(log);
            }
        }

        /*
         * Private.
         */

        private IEnumerator CheckImpl(Action<bool> onComplete)
        {
            // Cloudflare - available in the West and Asia.
            var url = $"https://cp.cloudflare.com/generate_204?t={DateTime.UtcNow.Ticks}";
            var expectedContent = string.Empty;

            using (var request = UnityWebRequest.Get(url))
            {
                LogAndRecord($"Checking internet... URL: {url}");

                request.timeout = Timeout;

                yield return request.SendWebRequest();

                LogAndRecord($"Code: {request.responseCode} Result: '{request.result}' Content: '{request.downloadHandler.text}'");

                if (request.responseCode == 204)
                {
                    onComplete(true);
                    yield break;
                }
            }

            // Apple - including content check.
            url = "https://captive.apple.com/hotspot-detect.html";
            expectedContent = "Success";

            using (var request = UnityWebRequest.Get(url))
            {
                LogAndRecord($"Checking internet... URL: {url}");

                request.timeout = Timeout;

                yield return request.SendWebRequest();

                LogAndRecord($"Code: {request.responseCode} Result: '{request.result}' Content: '{request.downloadHandler.text}'");

                if (request.result == UnityWebRequest.Result.Success &&
                    request.responseCode == 200 &&
                    request.downloadHandler.text.Contains(expectedContent))
                {
                    onComplete(true);
                    yield break;
                }
            }

            onComplete(false);
        }

        private void LogAndRecord(string message)
        {
            Log.Debug(message);

            _logs.Add(message);
        }
    }
}