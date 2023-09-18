using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.Utils.Pooling;
using UnityEngine.Events;

namespace Build1.PostMVC.Unity.App.Mediation
{
    public abstract class UnityViewDispatcher : UnityView, IEventDispatcher
    {
        private readonly EventDispatcher                    _dispatcher;
        private readonly Dictionary<UnityEventBase, object> _bindings;

        private static readonly Pool<IUnityEventBinding> _bindingsPool = new();

        protected UnityViewDispatcher()
        {
            _dispatcher = new EventDispatcher();
            _bindings = new Dictionary<UnityEventBase, object>();
        }

        /*
         * Event Dispatcher.
         */

        public void AddListener(Event @event, Action listener)                                     { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.AddListener(@event, listener); }

        public void AddListenerOnce(Event @event, Action listener)                                     { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.AddListenerOnce(@event, listener); }

        public bool ContainsListener(Event @event, Action listener)                                     { return _dispatcher.ContainsListener(@event, listener); }
        public bool ContainsListener<T1>(Event<T1> @event, Action<T1> listener)                         { return _dispatcher.ContainsListener(@event, listener); }
        public bool ContainsListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { return _dispatcher.ContainsListener(@event, listener); }
        public bool ContainsListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { return _dispatcher.ContainsListener(@event, listener); }

        public void RemoveListener(Event @event, Action listener)                                     { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.RemoveListener(@event, listener); }

        public void RemoveAllListeners()                 { _dispatcher.RemoveAllListeners(); }
        public void RemoveAllListeners(EventBase @event) { _dispatcher.RemoveAllListeners(@event); }

        public void Dispatch(Event @event)                                                             { _dispatcher.Dispatch(@event); }
        public void Dispatch<T1>(Event<T1> @event, T1 param01)                                         { _dispatcher.Dispatch(@event, param01); }
        public void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)                     { _dispatcher.Dispatch(@event, param01, param02); }
        public void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03) { _dispatcher.Dispatch(@event, param01, param02, param03); }

        /*
         * UnityEventBinding00 Events.
         */

        protected IUnityEventBinding BindUnityEvent(UnityEvent unityEvent, Event @event)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, _dispatcher, @event);
        }

        protected IUnityEventBinding BindUnityEvent<T1>(UnityEvent unityEvent, Event<T1> @event, T1 param01)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, _dispatcher, @event, param01);
        }
        
        protected IUnityEventBinding BindUnityEvent<T1>(UnityEvent unityEvent, Event<T1> @event, Func<T1> param01)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, _dispatcher, @event, param01);
        }

        protected IUnityEventBinding BindUnityEvent<T1, T2>(UnityEvent unityEvent, Event<T1, T2> @event, T1 param01, T2 param02)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, _dispatcher, @event, param01, param02);
        }

        protected IUnityEventBinding BindUnityEvent<T1, T2, T3>(UnityEvent unityEvent, Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, _dispatcher, @event, param01, param02, param03);
        }

        /*
         * UnityEventBinding00 Actions.
         */
        
        protected IUnityEventBinding BindUnityEvent(UnityEvent unityEvent, Action action)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, action);
        }

        protected IUnityEventBinding BindUnityEvent<T1>(UnityEvent unityEvent, Action<T1> action, T1 param01)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, action, param01);
        }

        protected IUnityEventBinding BindUnityEvent<T1, T2>(UnityEvent unityEvent, Action<T1, T2> action, T1 param01, T2 param02)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, action, param01, param02);
        }

        protected IUnityEventBinding BindUnityEvent<T1, T2, T3>(UnityEvent unityEvent, Action<T1, T2, T3> action, T1 param01, T2 param02, T3 param03)
        {
            return AddBinding<UnityEventBinding00>(unityEvent).Setup(unityEvent, action, param01, param02, param03);
        }

        /*
         * UnityEventBinding01.
         */

        protected IUnityEventBinding BindUnityEvent<T1>(UnityEvent<T1> unityEvent, Event<T1> @event)
        {
            return AddBinding<UnityEventBinding01<T1>>(unityEvent).Setup(unityEvent, _dispatcher, @event);
        }
        
        protected IUnityEventBinding BindUnityEvent<T1>(UnityEvent<T1> unityEvent, Action<T1> action)
        {
            return AddBinding<UnityEventBinding01<T1>>(unityEvent).Setup(unityEvent, action);
        }

        /*
         * Binding to UnityEvent with 2 params.
         */

        protected IUnityEventBinding BindUnityEvent<T1, T2>(UnityEvent<T1, T2> unityEvent, Event<T1, T2> @event)
        {
            return AddBinding<UnityEventBinding02<T1, T2>>(unityEvent).Setup(unityEvent, _dispatcher, @event);
        }
        
        protected IUnityEventBinding BindUnityEvent<T1, T2>(UnityEvent<T1, T2> unityEvent, Action<T1, T2> action)
        {
            return AddBinding<UnityEventBinding02<T1, T2>>(unityEvent).Setup(unityEvent, action);
        }

        /*
         * Binding to UnityEvent with 3 params.
         */

        protected IUnityEventBinding BindUnityEvent<T1, T2, T3>(UnityEvent<T1, T2, T3> unityEvent, Event<T1, T2, T3> @event)
        {
            return AddBinding<UnityEventBinding03<T1, T2, T3>>(unityEvent).Setup(unityEvent, _dispatcher, @event);
        }
        
        protected IUnityEventBinding BindUnityEvent<T1, T2, T3>(UnityEvent<T1, T2, T3> unityEvent, Action<T1, T2, T3> action)
        {
            return AddBinding<UnityEventBinding03<T1, T2, T3>>(unityEvent).Setup(unityEvent, action);
        }

        /*
         * Unbinding.
         */

        protected void UnbindUnityEvent(UnityEventBase unityEvent)
        {
            if (!_bindings.TryGetValue(unityEvent, out var bindingOrBindings))
                return;

            UnbindBindingOrBindings(bindingOrBindings);

            _bindings.Remove(unityEvent);
        }

        protected void UnbindUnityEvent(IUnityEventBinding binding)
        {
            if (!_bindings.TryGetValue(binding.UnityEvent, out var bindingOrBindings))
                return;

            if (bindingOrBindings == binding)
            {
                binding.Unbind();

                _bindings.Remove(binding.UnityEvent);
                _bindingsPool.Return(binding);
            }
            else
            {
                var bindings = (List<IUnityEventBinding>)bindingOrBindings;
                if (bindings.Remove(binding))
                {
                    if (bindings.Count == 0)
                        _bindings.Remove(binding.UnityEvent);

                    binding.Unbind();
                    _bindingsPool.Return(binding);
                }
            }
        }

        protected void UnbindAllUnityEvents()
        {
            foreach (var bindingOrBindings in _bindings.Values)
                UnbindBindingOrBindings(bindingOrBindings);
            _bindings.Clear();
        }

        /*
         * Private.
         */

        private T AddBinding<T>(UnityEventBase unityEvent) where T : IUnityEventBinding, new()
        {
            var binding = _bindingsPool.Take<T>();

            if (_bindings.TryGetValue(unityEvent, out var bindings))
            {
                if (bindings is IUnityEventBinding existingBinding)
                {
                    _bindings[unityEvent] = new List<IUnityEventBinding> { existingBinding, binding };
                }
                else
                {
                    ((List<IUnityEventBinding>)bindings).Add(binding);
                }
            }
            else
            {
                _bindings[unityEvent] = binding;
            }

            return binding;
        }

        private void UnbindBindingOrBindings(object bindingOrBindings)
        {
            if (bindingOrBindings is IUnityEventBinding binding)
            {
                binding.Unbind();
                _bindingsPool.Return(binding);
            }
            else
            {
                var bindings = (List<IUnityEventBinding>)bindingOrBindings;
                foreach (var item in bindings)
                {
                    item.Unbind();
                    _bindingsPool.Return(item);
                }
            }
        }
    }
}