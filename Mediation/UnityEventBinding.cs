using System;
using Build1.PostMVC.Core.MVCS.Events;
using UnityEngine.Events;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Build1.PostMVC.Unity.App.Mediation
{
    public sealed class UnityEventBinding00 : IUnityEventBinding
    {
        public UnityEventBase UnityEvent => _unityEvent;

        private IEventDispatcher _dispatcher;
        private UnityEvent       _unityEvent;
        private Action           _action;
        private Event            _event;
        private UnityAction      _listener;

        /*
         * Events.
         */

        internal IUnityEventBinding Setup(UnityEvent unityEvent, IEventDispatcher dispatcher, Event @event)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _event = @event;
            _listener = DispatchEvent;
            _unityEvent.AddListener(DispatchEvent);
            return this;
        }

        internal IUnityEventBinding Setup<T1>(UnityEvent unityEvent, IEventDispatcher dispatcher, Event<T1> @event, T1 param01)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _listener = () => { _dispatcher.Dispatch(@event, param01); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        internal IUnityEventBinding Setup<T1>(UnityEvent unityEvent, IEventDispatcher dispatcher, Event<T1> @event, Func<T1> param01Getter)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _listener = () => { _dispatcher.Dispatch(@event, param01Getter.Invoke()); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        internal IUnityEventBinding Setup<T1, T2>(UnityEvent unityEvent, IEventDispatcher dispatcher, Event<T1, T2> @event, T1 param01, T2 param02)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _listener = () => { _dispatcher.Dispatch(@event, param01, param02); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        internal IUnityEventBinding Setup<T1, T2, T3>(UnityEvent unityEvent, IEventDispatcher dispatcher, Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _listener = () => { _dispatcher.Dispatch(@event, param01, param02, param03); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        /*
         * Actions.
         */

        internal IUnityEventBinding Setup(UnityEvent unityEvent, Action action)
        {
            _unityEvent = unityEvent;
            _action = action;
            _listener = CallAction;
            _unityEvent.AddListener(CallAction);
            return this;
        }

        internal IUnityEventBinding Setup<T1>(UnityEvent unityEvent, Action<T1> action, T1 param01)
        {
            _unityEvent = unityEvent;
            _listener = () => { action.Invoke(param01); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        internal IUnityEventBinding Setup<T1, T2>(UnityEvent unityEvent, Action<T1, T2> action, T1 param01, T2 param02)
        {
            _unityEvent = unityEvent;
            _listener = () => { action.Invoke(param01, param02); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        internal IUnityEventBinding Setup<T1, T2, T3>(UnityEvent unityEvent, Action<T1, T2, T3> action, T1 param01, T2 param02, T3 param03)
        {
            _unityEvent = unityEvent;
            _listener = () => { action.Invoke(param01, param02, param03); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        /*
         * Unbinding.
         */

        public void Unbind()
        {
            if (_listener != null)
            {
                _unityEvent.RemoveListener(_listener);
                _unityEvent = null;

                _listener = null;
                _event = null;
                _action = null;
            }

            _unityEvent = null;
            _dispatcher = null;
        }

        /*
         * Private.
         */

        private void DispatchEvent() { _dispatcher.Dispatch(_event); }
        private void CallAction()    { _action?.Invoke(); }
    }

    public sealed class UnityEventBinding01<T1> : IUnityEventBinding
    {
        public UnityEventBase UnityEvent => _unityEvent;

        private IEventDispatcher _dispatcher;
        private UnityEvent<T1>   _unityEvent;
        private Action<T1>       _action;
        private Event<T1>        _event;
        private UnityAction<T1>  _listener;

        /*
         * Events.
         */

        internal IUnityEventBinding Setup(UnityEvent<T1> unityEvent, IEventDispatcher dispatcher, Event<T1> @event)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _event = @event;
            _listener = DispatchEvent;
            _unityEvent.AddListener(DispatchEvent);
            return this;
        }

        internal IUnityEventBinding Setup<P1>(UnityEvent<T1> unityEvent, IEventDispatcher dispatcher, Event<P1> @event, P1 param01)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _listener = _ => { _dispatcher.Dispatch(@event, param01); };
            _unityEvent.AddListener(_listener);
            return this;
        }

        /*
         * Actions.
         */

        internal IUnityEventBinding Setup(UnityEvent<T1> unityEvent, Action<T1> action)
        {
            _unityEvent = unityEvent;
            _action = action;
            _listener = CallAction;
            _unityEvent.AddListener(CallAction);
            return this;
        }

        /*
         * Unbinding.
         */

        public void Unbind()
        {
            if (_listener != null)
            {
                _unityEvent.RemoveListener(_listener);
                _unityEvent = null;

                _listener = null;

                _event = null;
                _action = null;
            }

            _unityEvent = null;
            _dispatcher = null;
        }

        /*
         * Event Handlers.
         */

        private void DispatchEvent(T1 param01) { _dispatcher.Dispatch(_event, param01); }
        private void CallAction(T1 param01)    { _action?.Invoke(param01); }
    }

    public sealed class UnityEventBinding02<T1, T2> : IUnityEventBinding
    {
        public UnityEventBase UnityEvent => _unityEvent;

        private IEventDispatcher    _dispatcher;
        private UnityEvent<T1, T2>  _unityEvent;
        private Event<T1, T2>       _event;
        private Action<T1, T2>      _action;
        private UnityAction<T1, T2> _listener;

        internal IUnityEventBinding Setup(UnityEvent<T1, T2> unityEvent, IEventDispatcher dispatcher, Event<T1, T2> @event)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _event = @event;
            _listener = DispatchEvent;
            _unityEvent.AddListener(DispatchEvent);
            return this;
        }

        internal IUnityEventBinding Setup(UnityEvent<T1, T2> unityEvent, Action<T1, T2> action)
        {
            _unityEvent = unityEvent;
            _action = action;
            _listener = CallAction;
            _unityEvent.AddListener(CallAction);
            return this;
        }

        public void Unbind()
        {
            if (_listener != null)
            {
                _unityEvent.RemoveListener(_listener);
                _unityEvent = null;

                _listener = null;

                _event = null;
                _action = null;
            }

            _unityEvent = null;
            _dispatcher = null;
        }

        /*
         * Event Handlers.
         */

        private void DispatchEvent(T1 param01, T2 param02) { _dispatcher.Dispatch(_event, param01, param02); }
        private void CallAction(T1 param01, T2 param02)    { _action?.Invoke(param01, param02); }
    }

    public sealed class UnityEventBinding03<T1, T2, T3> : IUnityEventBinding
    {
        public UnityEventBase UnityEvent => _unityEvent;

        private IEventDispatcher        _dispatcher;
        private UnityEvent<T1, T2, T3>  _unityEvent;
        private Event<T1, T2, T3>       _event;
        private Action<T1, T2, T3>      _action;
        private UnityAction<T1, T2, T3> _listener;

        internal IUnityEventBinding Setup(UnityEvent<T1, T2, T3> unityEvent, IEventDispatcher dispatcher, Event<T1, T2, T3> @event)
        {
            _unityEvent = unityEvent;
            _dispatcher = dispatcher;
            _event = @event;
            _listener = DispatchEvent;
            _unityEvent.AddListener(DispatchEvent);
            return this;
        }

        internal IUnityEventBinding Setup(UnityEvent<T1, T2, T3> unityEvent, Action<T1, T2, T3> action)
        {
            _unityEvent = unityEvent;
            _action = action;
            _listener = CallAction;
            _unityEvent.AddListener(CallAction);
            return this;
        }

        public void Unbind()
        {
            if (_listener != null)
            {
                _unityEvent.RemoveListener(_listener);
                _unityEvent = null;

                _listener = null;

                _event = null;
                _action = null;
            }

            _unityEvent = null;
            _dispatcher = null;
        }

        /*
         * Event Handlers.
         */

        private void DispatchEvent(T1 param01, T2 param02, T3 param03) { _dispatcher.Dispatch(_event, param01, param02, param03); }
        private void CallAction(T1 param01, T2 param02, T3 param03)    { _action?.Invoke(param01, param02, param03); }
    }
}