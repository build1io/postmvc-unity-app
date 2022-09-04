using System;
using Build1.PostMVC.Unity.App.Modules.UI;

namespace Build1.PostMVC.Unity.App.Modules.Screens
{
    public abstract class ScreenBase : UIControl<ScreenConfig>
    {
        internal readonly Type dataType;
        
        protected ScreenBase(string name, UIBehavior behavior) : base(name, behavior)
        {
        }
        
        protected ScreenBase(string name, UIBehavior behavior, Type dataType) : base(name, behavior)
        {
            this.dataType = dataType;
        }
    }
}