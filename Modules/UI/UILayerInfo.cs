using System;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI
{
    [Serializable]
    public sealed class UILayerInfo
    {
        [SerializeField] private int        id;
        [SerializeField] private GameObject gameObject;

        public int        Id         => id;
        public GameObject GameObject => gameObject;
    }
}