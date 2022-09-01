using Build1.PostMVC.Unity.App.Modules.UI;
using Build1.PostMVC.Unity.App.Modules.UI.Impl;

namespace Build1.PostMVC.Unity.App.Modules.HUD
{
    public abstract class HUDControllerBase : UIControlsController<HUDControl, HUDControlConfig>
    {
        protected void Show(HUDControl control)
        {
            GetInstance(control, UIControlOptions.Instantiate | UIControlOptions.Activate);
        }

        protected void Hide(HUDControl control)
        {
            Deactivate(control);
        }
    }
}