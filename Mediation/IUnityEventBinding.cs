using UnityEngine.Events;

namespace Build1.PostMVC.Unity.App.Mediation
{
    public interface IUnityEventBinding
    {
        UnityEventBase UnityEvent { get; }

        void Unbind();
    }
}