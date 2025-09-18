using System.Collections.Generic;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Layers
{
    public interface IUILayersController
    {
        bool LayersRegistered { get; }
        
        void RegisterLayers(IEnumerable<UILayerInfo> layers);

        GameObject GetLayerView(int layerId);
        T          GetLayerView<T>(int layerId) where T : Component;
    }
}