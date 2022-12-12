using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.App;
using Build1.PostMVC.Unity.App.Modules.Async;
using Build1.PostMVC.Unity.App.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.Device.Impl
{
    public abstract class DeviceControllerBase : IDeviceController
    {
        private const float OrientationCheckTimeout = 0.2F;

        [Log(LogLevel.Warning)] public ILog             Log           { get; set; }
        [Inject]                public IAsyncResolver   AsyncResolver { get; set; }
        [Inject]                public IEventDispatcher Dispatcher    { get; set; }

        public DevicePlatform          DevicePlatform          { get; private set; }
        public DeviceType              DeviceType              { get; private set; }
        public DeviceOrientation       DeviceOrientation       { get; private set; }
        public DeviceScreenOrientation DeviceScreenOrientation { get; private set; }

        public bool IsMobile => DevicePlatform == DevicePlatform.iOS ||
                                DevicePlatform == DevicePlatform.Android;

        public bool IsDesktop => DevicePlatform == DevicePlatform.OSX ||
                                 DevicePlatform == DevicePlatform.Windows ||
                                 DevicePlatform == DevicePlatform.Linux ||
                                 DevicePlatform == DevicePlatform.WebGL;

        public bool IsConsole => DevicePlatform == DevicePlatform.PS4 ||
                                 DevicePlatform == DevicePlatform.PS5 ||
                                 DevicePlatform == DevicePlatform.XboxOne ||
                                 DevicePlatform == DevicePlatform.Switch;

        public bool IsTV => DevicePlatform == DevicePlatform.tvOS;

        public bool IsPhone  => DeviceType == DeviceType.Phone;
        public bool IsTablet => DeviceType == DeviceType.Tablet;
        public bool IsWeb    => DevicePlatform == DevicePlatform.WebGL;

        private int                           _intervalId;
        private DeviceScreenOrientation       _orientations;
        private UnityEngine.DeviceOrientation _deviceOrientation;
        private ScreenOrientation             _screenOrientation;

        [PostConstruct]
        public void PostConstruct()
        {
            DevicePlatform = GetCurrentPlatform();
            DeviceType = GetDeviceType(DevicePlatform);
            DeviceOrientation = ToDeviceOrientation(Input.deviceOrientation);
            DeviceScreenOrientation = ToScreenOrientation(Screen.orientation);

            Log.Debug((p, t) => $"Platform: {p} DeviceType: {t}", DevicePlatform, DeviceType);
            Log.Debug(() => "Diagonal: " + GetDiagonalInches() + " AspectRatio: " + GetAspectRatio());

            Dispatcher.AddListener(AppEvent.Pause, OnAppPause);

            StartCheck();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Dispatcher.RemoveListener(AppEvent.Pause, OnAppPause);

            StopCheck();
        }

        /*
         * Public.
         */

        public T GetConfiguration<T>(IEnumerable<T> configurations) where T : IDeviceDependentConfiguration
        {
            foreach (var configuration in configurations)
            {
                if (configuration.DevicePlatform != DevicePlatform.Any && configuration.DevicePlatform != DevicePlatform)
                    continue;
                if (configuration.DeviceType != DeviceType.Any && configuration.DeviceType != DeviceType)
                    continue;
                return configuration;
            }

            throw new Exception($"No suitable configuration found: platform: {DevicePlatform} device: {DeviceType}");
        }

        public void SetAvailableOrientations(DeviceScreenOrientation orientations)
        {
            if (_orientations == orientations)
                return;

            _orientations = orientations;
            
            Log.Debug(o => $"Setting available orientations: {o}", _orientations);

            switch (_orientations)
            {
                case DeviceScreenOrientation.Portrait:
                    Screen.orientation = ScreenOrientation.Portrait;
                    return;
                case DeviceScreenOrientation.PortraitUpsideDown:
                    Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                    return;
                case DeviceScreenOrientation.LandscapeLeft:
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    return;
                case DeviceScreenOrientation.LandscapeRight:
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                    return;
            }

            var autorotateToPortrait = (_orientations & DeviceScreenOrientation.Portrait) == DeviceScreenOrientation.Portrait;
            var autorotateToPortraitUpsideDown = (_orientations & DeviceScreenOrientation.PortraitUpsideDown) == DeviceScreenOrientation.PortraitUpsideDown;
            var autorotateToLandscapeLeft = (_orientations & DeviceScreenOrientation.LandscapeLeft) == DeviceScreenOrientation.LandscapeLeft;
            var autorotateToLandscapeRight = (_orientations & DeviceScreenOrientation.LandscapeRight) == DeviceScreenOrientation.LandscapeRight;
            
            if (Screen.orientation != ScreenOrientation.AutoRotation)
            {
                if (autorotateToPortrait)
                    Screen.orientation = ScreenOrientation.Portrait;
                else if (autorotateToLandscapeLeft)
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                else if (autorotateToLandscapeRight)
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                else if (autorotateToPortraitUpsideDown)
                    Screen.orientation = ScreenOrientation.PortraitUpsideDown;
            }
            
            Screen.autorotateToPortrait = autorotateToPortrait;
            Screen.autorotateToPortraitUpsideDown = autorotateToPortraitUpsideDown;
            Screen.autorotateToLandscapeLeft = autorotateToLandscapeLeft;
            Screen.autorotateToLandscapeRight = autorotateToLandscapeRight;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        /*
         * Protected.
         */

        protected abstract DevicePlatform GetCurrentPlatform();

        /*
         * Private.
         */

        private void StartCheck()
        {
            Log.Debug("StartCheck");

            if (_intervalId == AsyncResolver.DefaultCallId)
                _intervalId = AsyncResolver.IntervalCall(Check, OrientationCheckTimeout);
        }

        private void StopCheck()
        {
            Log.Debug("StopCheck");

            AsyncResolver.CancelCall(ref _intervalId);
        }

        private void Check()
        {
            var deviceType = GetDeviceType(DevicePlatform);
            if (DeviceType != deviceType)
            {
                Log.Debug(o => $"Device Type Changed: {o}", deviceType);

                DeviceType = deviceType;

                Dispatcher.Dispatch(DeviceEvent.DeviceTypeChanged, DeviceType);
            }

            if (_deviceOrientation != Input.deviceOrientation)
            {
                Log.Debug(o => $"Device orientation chanced: {o}", Input.deviceOrientation);

                _deviceOrientation = Input.deviceOrientation;
                DeviceOrientation = ToDeviceOrientation(Input.deviceOrientation);

                Dispatcher.Dispatch(DeviceEvent.DeviceOrientationChanged, DeviceOrientation);
            }

            if (_screenOrientation != Screen.orientation)
            {
                Log.Debug(o => $"Screen orientation changed: {o}", Screen.orientation);

                _screenOrientation = Screen.orientation;
                DeviceScreenOrientation = ToScreenOrientation(_screenOrientation);

                Dispatcher.Dispatch(DeviceEvent.ScreenOrientationChanged, DeviceScreenOrientation);
            }
        }

        private DeviceType GetDeviceType(DevicePlatform platform)
        {
            return platform switch
            {
                DevicePlatform.iOS     => GetMobileDeviceType(),
                DevicePlatform.Android => GetMobileDeviceType(),
                DevicePlatform.OSX     => DeviceType.Desktop,
                DevicePlatform.Windows => DeviceType.Desktop,
                DevicePlatform.Linux   => DeviceType.Desktop,
                DevicePlatform.WebGL   => DeviceType.Desktop,
                DevicePlatform.XboxOne => DeviceType.Console,
                DevicePlatform.PS4     => DeviceType.Console,
                DevicePlatform.PS5     => DeviceType.Console,
                DevicePlatform.Switch  => DeviceType.Console,
                DevicePlatform.tvOS    => DeviceType.TV,
                _                      => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
            };
        }

        private DeviceType GetMobileDeviceType()
        {
            if (GetDiagonalInches() >= 7F && GetAspectRatio() <= 1.77)
                return DeviceType.Tablet;
            return DeviceType.Phone;
        }

        private float GetAspectRatio()
        {
            return Mathf.Max((float)Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
        }

        private float GetDiagonalInches()
        {
            var screenWidth = Screen.width / Screen.dpi;
            var screenHeight = Screen.height / Screen.dpi;
            return Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        }

        private DeviceOrientation ToDeviceOrientation(UnityEngine.DeviceOrientation orientation)
        {
            return orientation switch
            {
                UnityEngine.DeviceOrientation.Portrait           => DeviceOrientation.Portrait,
                UnityEngine.DeviceOrientation.PortraitUpsideDown => DeviceOrientation.PortraitUpsideDown,
                UnityEngine.DeviceOrientation.LandscapeLeft      => DeviceOrientation.LandscapeLeft,
                UnityEngine.DeviceOrientation.LandscapeRight     => DeviceOrientation.LandscapeRight,
                UnityEngine.DeviceOrientation.Unknown            => DeviceOrientation.Unknown,
                UnityEngine.DeviceOrientation.FaceUp             => DeviceOrientation.FaceUp,
                UnityEngine.DeviceOrientation.FaceDown           => DeviceOrientation.FaceDown,
                _                                                => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        private DeviceScreenOrientation ToScreenOrientation(ScreenOrientation orientation)
        {
            return orientation switch
            {
                ScreenOrientation.Portrait           => DeviceScreenOrientation.Portrait,
                ScreenOrientation.PortraitUpsideDown => DeviceScreenOrientation.PortraitUpsideDown,
                ScreenOrientation.LandscapeLeft      => DeviceScreenOrientation.LandscapeLeft,
                ScreenOrientation.LandscapeRight     => DeviceScreenOrientation.LandscapeRight,
                _                                    => DeviceScreenOrientation.Unknown
            };
        }

        /*
         * Events Handlers.
         */

        private void OnAppPause(bool paused)
        {
            if (!paused)
                Check();
        }
    }
}