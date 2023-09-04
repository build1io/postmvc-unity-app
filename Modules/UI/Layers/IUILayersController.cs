using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Layers
{
    public interface IUILayersController
    {
        void RegisterLayer(int layerId, GameObject view);
        void ResetLayers();

        GameObject GetLayerView(int layerId);
        T          GetLayerView<T>(int layerId) where T : Component;
    }
}