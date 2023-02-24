using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Logging.Impl;

namespace Build1.PostMVC.Unity.App.Modules.Logging
{
    public sealed class LogProvider : InjectionProvider<ILog, LogAttribute>
    {
        [Inject] public ILogController LogController { get; set; }
        
        private readonly Stack<ILog> _availableInstances;
        private readonly List<ILog>  _usedInstances;

        public LogProvider()
        {
            _availableInstances = new Stack<ILog>();
            _usedInstances = new List<ILog>();
        }

        /*
         * Provider.
         */

        public override ILog TakeInstance(object owner, LogAttribute attribute)
        {
            ILog log;

            if (_availableInstances.Count > 0)
            {
                log = _availableInstances.Pop();
                log.SetPrefix(owner.GetType().Name);
                log.SetLevel(attribute.logLevel);
                _usedInstances.Add(log);
            }
            else
            {
                log = GetLogImpl(owner.GetType().Name, attribute.logLevel, LogController);
                _usedInstances.Add(log);
            }

            return log;
        }

        public override void ReturnInstance(ILog instance)
        {
            if (_usedInstances.Remove(instance))
                _availableInstances.Push(instance);
        }
        
        /*
         * Static.
         */
        
        public static ILog GetLog<T>(LogLevel level)
        {
            var log = GetLogImpl(typeof(T).Name, level, Core.PostMVC.GetInstance<ILogController>());

            if (typeof(UnityEngine.MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                log.Warn("You're getting a logger during MonoBehavior instantiation. " +
                         "This may end up in script instantiation exception on a device. " +
                         "Consider inheriting of component from UnityView and injecting a logger.");
            }

            return log;
        }

        public static ILog GetLog(LogLevel level)
        {
            return GetLogImpl(null, level, Core.PostMVC.GetInstance<ILogController>());
        }
        
        public static ILog GetLog(string prefix, LogLevel level)
        {
            return GetLogImpl(prefix, level, Core.PostMVC.GetInstance<ILogController>());
        }
        
        public static ILog GetLog(object owner, LogLevel level)
        {
            return GetLogImpl(owner.GetType().Name, level, Core.PostMVC.GetInstance<ILogController>());
        }

        internal static ILog GetLogImpl(string prefix, LogLevel level, ILogController logController)
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
        
            LogWebGL requires major update.
            return new LogWebGL(prefix, level, logController);

            #else

            return new LogDefault(prefix, level, logController);

            #endif

        }
    }
}