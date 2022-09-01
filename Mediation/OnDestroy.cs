using System;
using Build1.PostMVC.Core.Extensions.MVCS.Injection;

namespace Build1.PostMVC.UnityApp.Mediation
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnDestroy : PreDestroy
    {
    }
}