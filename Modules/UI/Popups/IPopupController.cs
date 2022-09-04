namespace Build1.PostMVC.Unity.App.Modules.UI.Popups
{
    public interface IPopupController
    {
        bool HasOpenPopups { get; }

        void Open(Popup popup);
        void Open(Popup popup, PopupBehavior behavior);
        
        void Open<T>(Popup<T> popup, T data);
        void Open<T>(Popup<T> popup, T data, PopupBehavior behavior);
        
        void Close(PopupBase popup, bool immediate = false);
        void CloseAll(bool immediate = false);

        bool CheckPopupOpen(PopupBase popup);
        bool CheckPopupOnTop(IPopupView popupView);
    }
}