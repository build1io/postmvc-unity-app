namespace Build1.PostMVC.UnityApp.Modules.Popups
{
    public enum PopupBehavior
    {
        /// Popup will be shown or added to queue if there is an active popup.
        Default = 1,
        
        /// Popup will be open on top of the current one.
        OpenOnTop = 2
    }
}