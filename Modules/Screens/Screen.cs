using Build1.PostMVC.UnityApp.Modules.UI;

namespace Build1.PostMVC.UnityApp.Modules.Screens
{
    public sealed class Screen : ScreenBase
    {
        public Screen(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
    }
    
    public sealed class Screen<T> : ScreenBase
    {
        public Screen(string name, UIControlBehavior behavior) : base(name, behavior, typeof(T))
        {
        }
    }
}