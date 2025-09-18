using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.UI.Layers;
using Build1.PostMVC.Unity.App.Modules.Update;

namespace Build1.PostMVC.Unity.App.Modules.UI.Commands
{
    public sealed class WaitUILayersCommand : Command
    {
        [Inject] public IUILayersController UILayersController { get; set; }
        [Inject] public IUpdateController   UpdateController   { get; set; }

        public override void Execute()
        {
            if (UILayersController.LayersRegistered)
                return;

            Retain();

            UpdateController.SubscribeForUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime)
        {
            if (!UILayersController.LayersRegistered)
                return;

            UpdateController.Unsubscribe(OnUpdate);

            Release();
        }
    }
}