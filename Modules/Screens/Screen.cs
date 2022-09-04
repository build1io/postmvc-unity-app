using Build1.PostMVC.Unity.App.Modules.UI;

namespace Build1.PostMVC.Unity.App.Modules.Screens
{
    public sealed class Screen : ScreenBase
    {
        public Screen(string name, UIBehavior behavior) : base(name, behavior)
        {
        }
    }
    
    public sealed class Screen<T> : ScreenBase
    {
        public Screen(string name, UIBehavior behavior) : base(name, behavior, typeof(T))
        {
        }
    }
}