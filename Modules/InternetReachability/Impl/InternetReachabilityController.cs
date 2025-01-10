using System;
using System.Collections;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Contexts;
using UnityEngine;
using UnityEngine.Networking;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability.Impl
{
    public sealed class InternetReachabilityController : IInternetReachabilityController
    {
        [Inject] public IContextView ContextView { get; set; }

        public bool LastResult      { get; private set; }
        public long LastResultCode  { get; private set; } = -1;
        public long EmptyResultCode { get; }              = -1;

        private MonoBehaviour _coroutineProvider;

        [PostConstruct]
        public void PostConstruct()
        {
            _coroutineProvider = ContextView.As<MonoBehaviour>();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            _coroutineProvider = null;
        }

        /*
         * Public.
         */

        public void Check(Action<bool> onComplete = null)
        {
            _coroutineProvider.StartCoroutine(CheckImpl("https://google.com", onComplete, 3));
        }


        public void Check(int timeout, Action<bool> onComplete = null)
        {
            _coroutineProvider.StartCoroutine(CheckImpl("https://google.com", onComplete, timeout));
        }

        public void Check(string url, Action<bool> onComplete = null)
        {
            _coroutineProvider.StartCoroutine(CheckImpl(url, onComplete, 3));
        }

        public void Check(string url, int timeout, Action<bool> onComplete = null)
        {
            _coroutineProvider.StartCoroutine(CheckImpl(url, onComplete, timeout));
        }

        /*
         * Private.
         */

        private IEnumerator CheckImpl(string url, Action<bool> onComplete, int timeout)
        {
            bool result;
            using (var request = UnityWebRequest.Head(url))
            {
                request.timeout = timeout;
                yield return request.SendWebRequest();
                result = request.responseCode == 200;

                LastResult = result;
                LastResultCode = request.responseCode;
            }

            if (_coroutineProvider != null)
                onComplete?.Invoke(result);
        }
    }
}