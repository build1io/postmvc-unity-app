namespace Build1.PostMVC.Unity.App.Modules.UI.HUD
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