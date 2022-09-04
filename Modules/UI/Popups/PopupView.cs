using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.UI.Popups.Animation;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Popups
{
    public abstract class PopupView : UnityView, IPopupView
    {
        [Header("Parts Base"), SerializeField] private GameObject     overlay;
        [SerializeField]                       private RectTransform  content;
        [SerializeField]                       private GameObject     raycastBlocker;
        [SerializeField]                       private PopupAnimation animationObject;

        [Inject] public IPopupController PopupController { get; set; }

        public PopupBase  Popup      { get; private set; }
        public GameObject GameObject => gameObject;

        public GameObject    Overlay => overlay;
        public RectTransform Content => content;
        
        public bool InputBlocked => raycastBlocker && raycastBlocker.activeSelf;
        public bool IsAnimating  { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (animationObject)
            {
                if (raycastBlocker)
                    raycastBlocker.SetActive(true);

                animationObject.AnimateShow(this, OnShownImpl);
                IsAnimating = true;
                return;
            }

            OnShownImpl();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (animationObject)
            {
                if (raycastBlocker)
                    raycastBlocker.SetActive(false);
             
                animationObject.KillAnimations(this);
                IsAnimating = false;
            }
        }

        /*
         * Public.
         */

        public void SetUp(PopupBase popup)
        {
            Popup = popup;
        }

        public virtual void Close()
        {
            if (animationObject)
            {
                if (IsAnimating)
                    return;
                
                if (raycastBlocker)
                    raycastBlocker.SetActive(true);

                animationObject.AnimateHide(this, OnHiddenImpl);
                IsAnimating = true;
                return;
            }

            OnHiddenImpl();
        }

        /*
         * Protected.
         */

        protected virtual void OnShownHandler()  { }
        protected virtual void OnHiddenHandler() { }

        /*
         * Private.
         */

        private void OnShownImpl()
        {
            if (raycastBlocker)
                raycastBlocker.SetActive(false);
            
            IsAnimating = false;

            OnShownHandler();
        }

        private void OnHiddenImpl()
        {
            IsAnimating = false;
            
            OnHiddenHandler();
            
            PopupController.Close(Popup, true);
        }
    }
}