using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Unity.App.Modules.Logging.Commands
{
    [Poolable]
    public sealed class LogExceptionCommand : Command<ILog, Exception>
    {
        public override void Execute(ILog log, Exception exception)
        {
            log.Error(exception);
        }
    }
}