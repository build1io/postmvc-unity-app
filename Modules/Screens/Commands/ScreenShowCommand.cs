using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.App.Modules.Screens.Commands
{
    [Poolable]
    public sealed class ScreenShowCommand : Command<Screen>
    {
        [Inject] public IScreensController ScreensController { get; set; }
        
        public override void Execute(Screen screen)
        {
            if (!ScreensController.CheckScreenIsActive(screen))
                ScreensController.Show(screen);
        }
    }
}