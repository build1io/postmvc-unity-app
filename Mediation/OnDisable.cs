using System;

namespace Build1.PostMVC.UnityApp.Mediation
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnDisable : Attribute
    {
    }
}