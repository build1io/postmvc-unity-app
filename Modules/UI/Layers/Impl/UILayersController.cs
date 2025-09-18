using System;
using System.Collections.Generic;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Layers.Impl
{
    public sealed class UILayersController : IUILayersController
    {
        public bool LayersRegistered => _layers != null;
        
        private Dictionary<int, GameObject> _layers;

        /*
         * Public.
         */

        public void RegisterLayers(IEnumerable<UILayerInfo> layers)
        {
            if (_layers != null)
                throw new Exception($"Layers already registered");
            
            _layers = new Dictionary<int, GameObject>();
            
            foreach (var layer in layers)
            {
                if (_layers.ContainsKey(layer.Id))
                    throw new Exception($"Layer view already registered. Id: {layer.Id}");
                
                _layers.Add(layer.Id, layer.GameObject);    
            }
        }

        public GameObject GetLayerView(int layerId)
        {
            if (!_layers.TryGetValue(layerId, out var view))
                throw new Exception($"Layer not registered. Id: {layerId}");
            return view;
        }
        
        public T GetLayerView<T>(int layerId) where T : Component
        {
            if (!_layers.TryGetValue(layerId, out var view))
                throw new Exception($"Layer not registered: Id: {layerId}");

            if (typeof(T) == typeof(Transform))
                return view.transform as T;
            
            if (typeof(T) == typeof(GameObject))
                return view as T;
            
            throw new Exception($"Incompatible layer view type: {typeof(T)}");
        }
    }
}