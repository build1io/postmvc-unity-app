using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.App.Mediation.Impl
{
    public abstract class UnityEventBindingBase
    {
        protected IEventDispatcher _dispatcher;
        
        protected UnityEventBindingBase(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        public abstract void Destroy();
    }
}