namespace Build1.PostMVC.UnityApp.Mediation.Api
{
    internal interface IUnityViewEventProcessor
    {
        void ProcessStart(IUnityView view);
        void ProcessOnEnable(IUnityView view);
        void ProcessOnDisable(IUnityView view);
    }
}