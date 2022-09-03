using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;

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
                log = Logging.GetLogImpl(owner.GetType(), attribute.logLevel, LogController);
                _usedInstances.Add(log);
            }

            return log;
        }

        public override void ReturnInstance(ILog instance)
        {
            if (_usedInstances.Remove(instance))
                _availableInstances.Push(instance);
        }
    }
}