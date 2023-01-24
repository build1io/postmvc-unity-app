#if UNITY_ANDROID || UNITY_EDITOR

using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.Android
{
    public static class AndroidUtils
    {
        private static int _apiVersion;
        
        public static int GetCurrentAndroidAPIVersion()
        {
            if (_apiVersion != 0) 
                return _apiVersion;
            
            var clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
            var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
            _apiVersion = AndroidJNI.GetStaticIntField(clazz, fieldID);
            
            return _apiVersion;
        }
    }
}

#endif