using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Unity.App.Modules.Logging;

namespace Build1.PostMVC.Unity.App.Utils.Performance.Commands
{
    [Poolable]
    public sealed class PerformanceStartCommand : Command<string>
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }
        
        public override void Execute(string identifier)
        {
            PerformanceTool.Start(identifier);
            
            Log.Warn($"{identifier} measurement start");
        }
    }
}