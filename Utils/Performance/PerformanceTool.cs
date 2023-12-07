using System.Collections.Generic;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Utils.Performance
{
    public static class PerformanceTool
    {
        private static readonly Dictionary<object, float> _timings = new();
        
        public static void Start(object key)
        {
            _timings.Add(key, Time.realtimeSinceStartup);
        }
        
        public static float Stop(object key)
        {
            if (_timings.TryGetValue(key, out var time))
            {
                time = Time.realtimeSinceStartup - time;
                _timings.Remove(key);
            }
            else
            {
                time = -1;
            }
            
            return time;
        }
    }
}