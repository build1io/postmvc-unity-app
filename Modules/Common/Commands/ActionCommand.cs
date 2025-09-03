using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Unity.App.Modules.Common.Commands
{
    [Poolable]
    public sealed class ActionCommand : Command<Action>
    {
        public override void Execute(Action action)
        {
            action?.Invoke();
        }
    }
}