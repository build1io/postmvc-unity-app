using System;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Popups.Animation
{
    public abstract class PopupAnimation : ScriptableObject
    {
        public abstract void AnimateShow(IPopupView view, Action onComplete);
        public abstract void AnimateHide(IPopupView view, Action onComplete);
        public abstract void KillAnimations(IPopupView view);
    }
}