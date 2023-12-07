using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Unity.App.Modules.Logging;

namespace Build1.PostMVC.Unity.App.Utils.Performance.Commands
{
    [Poolable]
    public sealed class PerformanceStopCommand : Command<string>
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }
        
        public override void Execute(string identifier)
        {
            Log.Warn($"{identifier} done in {PerformanceTool.Stop(identifier)}s");
        }
    }
}