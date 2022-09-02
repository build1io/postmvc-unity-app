using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Unity.App.Modules.Logging.Commands
{
    [Poolable]
    public sealed class LogErrorCommand : Command<ILog, string>
    {
        public override void Execute(ILog log, string message)
        {
            log.Error(message);
        }
    }
}