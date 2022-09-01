using System;
using Build1.PostMVC.Core.Contexts;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Contexts.Impl
{
    internal sealed class ContextViewUnity : MonoBehaviour, IContextView
    {
        public IContext Context { get; private set; }
        public object   ViewRaw => this;

        private void OnApplicationQuit()
        {
            Context.SetQuitting();
        }
        
        #if UNITY_EDITOR
        
        /*
         * Added for debug needs, to test disposal processes.
         */
        public void OnDisable()
        {
            Context.Stop();
        }
        
        #endif
        
        /*
         * Public.
         */
        
        public void SetContext(IContext context)
        {
            Context = context;
        }

        public T As<T>() where T : class
        {
            if (typeof(T) == typeof(MonoBehaviour))
                return this as T;
            if (typeof(T) == typeof(GameObject))
                return gameObject as T;
            if (typeof(T) == typeof(Transform))
                return transform as T;
            throw new Exception($"Incompatible required type for view [{typeof(T).FullName}].");
        }
    }
}