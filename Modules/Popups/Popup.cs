using Build1.PostMVC.UnityApp.Modules.UI;

namespace Build1.PostMVC.UnityApp.Modules.Popups
{
    public sealed class Popup : PopupBase
    {
        public Popup(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
    }
    
    public sealed class Popup<T> : PopupBase
    {
        public Popup(string name, UIControlBehavior behavior) : base(name, behavior, typeof(T))
        {
        }
    }
}