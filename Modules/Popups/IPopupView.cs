using Build1.PostMVC.Unity.App.Mediation;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.Popups
{
    public interface IPopupView : IUnityView
    {
        PopupBase  Popup      { get; }
        GameObject GameObject { get; }

        GameObject    Overlay { get; }
        RectTransform Content { get; }

        void SetUp(PopupBase popup);
        void Close();
    }
}