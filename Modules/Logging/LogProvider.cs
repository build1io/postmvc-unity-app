using System;
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
                log.SetLevel(Logging.ForceLevel != LogLevel.None ? Logging.ForceLevel : attribute.logLevel);
                _usedInstances.Add(log);
            }
            else
            {
                log = GetLogImpl(owner.GetType(), attribute.logLevel, LogController);
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
            var log = GetLogImpl(typeof(T), level, Core.PostMVC.GetInstance<ILogController>());

            if (typeof(UnityEngine.MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                log.Warn("You're getting a logger during MonoBehavior instantiation. " +
                         "This may end up in script instantiation exception on a device. " +
                         "Consider inheriting of component from UnityView and injecting a logger.");
            }

            return log;
        }

        public static ILog GetLog(object owner, LogLevel level)
        {
            return GetLogImpl(owner.GetType(), level, Core.PostMVC.GetInstance<ILogController>());
        }

        internal static ILog GetLogImpl(Type ownerType, LogLevel level, ILogController logController)
        {
            if (Logging.Print || Logging.Record)
            {
                if (Logging.ForceLevel != LogLevel.None) // Using force level if it was set.
                    level = Logging.ForceLevel;
                else if (level > LogLevel.None && level < Logging.MinLevel) // Filtering logs by min log level (could be set different for production environment).
                    level = Logging.MinLevel;

                if (level != LogLevel.None)
                {
                    #if UNITY_WEBGL && !UNITY_EDITOR
                
                    return new LogWebGL(ownerType.Name, level, logController);

                    #else

                    return new LogDefault(ownerType.Name, level, logController);

                    #endif
                }    
            }
            
            return new LogVoid();
        }
    }
}