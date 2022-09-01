using Build1.PostMVC.Core.Extensions.MVCS.Mediation;

namespace Build1.PostMVC.UnityApp.Mediation
{
    public interface IUnityView : IView
    {
        bool Initialized { get; }
        bool Enabled     { get; }
    }
}