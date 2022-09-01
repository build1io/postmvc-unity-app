using Build1.PostMVC.Core.Contexts;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Agents;
using Build1.PostMVC.Unity.App.Modules.Logging;
using Build1.PostMVC.Unity.App.Utils.Path;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Build1.PostMVC.Unity.App.Modules.App.Impl
{
    public sealed class AppController : IAppController
    {
        [Log(LogLevel.Warning)] public  ILog              Log              { get; set; }
        [Inject]                public  IEventDispatcher  Dispatcher       { get; set; }
        [Inject]                public  IContext          Context          { get; set; }
        [Inject]                private IAgentsController AgentsController { get; set; }

        public string PersistentDataPath { get; private set; }
        public string Version            => Application.version;

        public bool IsPaused  { get; private set; }
        public bool IsFocused { get; private set; }

        private AppAgent _agent;
        private string   _mainSceneName;

        [PostConstruct]
        private void PostConstruct()
        {
            PersistentDataPath = GetPersistentDataPath();

            _mainSceneName = GetMainSceneName();

            _agent = AgentsController.Create<AppAgent>();
            _agent.Pause += OnPause;
            _agent.Focus += OnFocus;
            _agent.Quitting += OnQuitting;

            Log.Debug(s => $"MainScene: \"{s}\"", _mainSceneName);
        }

        [PreDestroy]
        private void PreDestroy()
        {
            _agent.Pause -= OnPause;
            _agent.Focus -= OnFocus;
            _agent.Quitting -= OnQuitting;
            AgentsController.Destroy(ref _agent);
        }

        /*
         * Public.
         */

        public void Restart()
        {
            // Signalling that app is about to restart.
            Dispatcher.Dispatch(AppEvent.Restarting);

            // Stopping the root context.
            // It must stop all nested all contexts and dispose all dependencies.
            Context.Stop();

            // Reloading the main scene.
            SceneManager.LoadScene(_mainSceneName);
        }

        /*
         * Private.
         */

        private string GetMainSceneName()
        {
            return System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0));
        }

        /*
         * Event handlers.
         */

        private void OnPause(bool paused)
        {
            if (IsPaused == paused)
                return;

            Log.Debug(p => $"OnPause({p})", paused);

            IsPaused = paused;
            Dispatcher.Dispatch(AppEvent.Pause, paused);
        }

        private void OnFocus(bool focused)
        {
            if (IsFocused == focused)
                return;

            Log.Debug(f => $"OnFocus({f})", focused);

            IsFocused = focused;
            Dispatcher.Dispatch(AppEvent.Focus, focused);
        }

        private void OnQuitting()
        {
            Dispatcher.Dispatch(AppEvent.Quitting);
        }

        /*
         * Static.
         */

        public static string GetPersistentDataPath()
        {
            return PathUtil.GetPath(PathAttribute.Internal | PathAttribute.Persistent | PathAttribute.Canonical);
        }
    }
}