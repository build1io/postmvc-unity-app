using UnityEngine;

namespace Build1.PostMVC.UnityApp.Modules.UI
{
    public interface IUILayersController
    {
        void RegisterLayer(int layerId, GameObject view);

        GameObject GetLayerView(int layerId);
        T          GetLayerView<T>(int layerId) where T : Component;
    }
}