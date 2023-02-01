using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Unity.App.Modules.Logging;
using Build1.PostMVC.Unity.App.Modules.Services.Impl;

namespace Build1.PostMVC.Unity.App.Modules.Services.Commands
{
    public sealed class UnityServicesInitializeCommand : Command
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }
        
        public override void Execute()
        {
            if (UnityServicesAdapter.Initialized)
            {
                Log.Debug("Unity services already initialized");
                return;
            }
            
            Retain();
            
            Log.Debug("Initializing...");

            UnityServicesAdapter.OnInitialized += OnInitialized;
            UnityServicesAdapter.OnError += OnError;
            UnityServicesAdapter.Initialize();
        }

        private void OnInitialized()
        {
            Log.Debug("Initialized");
            
            UnityServicesAdapter.OnInitialized -= OnInitialized;
            UnityServicesAdapter.OnError -= OnError;
            
            Release();
        }

        private void OnError(Exception exception)
        {
            Log.Error(exception);
            
            UnityServicesAdapter.OnInitialized -= OnInitialized;
            UnityServicesAdapter.OnError -= OnError;
            
            Release();
        }
    }
}