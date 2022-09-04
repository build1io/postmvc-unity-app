using Build1.PostMVC.Unity.App.Modules.UI;

namespace Build1.PostMVC.Unity.App.Modules.HUD
{
    public sealed class HUDControl : UIControl<HUDControlConfig>
    {
        public HUDControl(string name) : base(name, UIBehavior.Default)
        {
        }
        
        public HUDControl(string name, UIBehavior behavior) : base(name, behavior)
        {
        }
    }
}