using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Unity.App.Modules.Common.Commands
{
    [Poolable]
    public sealed class ActionCallbackCommand : Command<Action<Action>>
    {
        public override void Execute(Action<Action> action)
        {
            if (action == null)
                return;
            
            Retain();
            action.Invoke(Release);
        }
    }
}