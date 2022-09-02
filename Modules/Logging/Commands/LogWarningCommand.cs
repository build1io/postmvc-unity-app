using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Unity.App.Modules.Logging.Commands
{
    [Poolable]
    public sealed class LogWarningCommand : Command<ILog, string>
    {
        public override void Execute(ILog log, string message)
        {
            log.Warn(message);
        }
    }
}