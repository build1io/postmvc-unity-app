namespace Build1.PostMVC.Unity.App.Mediation.Api
{
    internal interface IUnityViewEventProcessor
    {
        void ProcessStart(IUnityView view);
        void ProcessOnEnable(IUnityView view);
        void ProcessOnDisable(IUnityView view);
    }
}