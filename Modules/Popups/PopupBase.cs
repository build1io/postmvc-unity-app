using System;
using Build1.PostMVC.UnityApp.Modules.UI;

namespace Build1.PostMVC.UnityApp.Modules.Popups
{
    public abstract class PopupBase : UIControl<PopupConfigBase>
    {
        public readonly Type dataType;

        protected PopupBase(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
        
        protected PopupBase(string name, UIControlBehavior behavior, Type dataType) : base(name, behavior)
        {
            this.dataType = dataType;
        }
    }
}