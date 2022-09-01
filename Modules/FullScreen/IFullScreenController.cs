namespace Build1.PostMVC.UnityApp.Modules.FullScreen
{
    public interface IFullScreenController
    {
        bool IsInFullScreen { get; }
        
        void ToggleFullScreen();
    }
}