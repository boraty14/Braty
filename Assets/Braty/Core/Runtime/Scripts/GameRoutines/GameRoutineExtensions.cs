using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.GameRoutines
{
    public static class GameRoutineExtensions
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            WaitForSecondsMap.Clear();
            WaitForSecondsRealtimeMap.Clear();
            WaitForEndOfFrame = new WaitForEndOfFrame();
            WaitForFixedUpdate = new WaitForFixedUpdate();
        }

        private static readonly Dictionary<float, WaitForSeconds> WaitForSecondsMap = new Dictionary<float, WaitForSeconds>();
        private static readonly Dictionary<float, WaitForSecondsRealtime> WaitForSecondsRealtimeMap = new Dictionary<float, WaitForSecondsRealtime>();
        public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        public static WaitUntilYield WaitUntil(Func<bool> callback) => new WaitUntilYield(callback);
        public static WaitUntilAllYield WaitUntilAll(List<Func<bool>> callbacks) => new WaitUntilAllYield(callbacks);
        public static WaitUntilOneOfYield WaitUntilOneOf(List<Func<bool>> callbacks) => new WaitUntilOneOfYield(callbacks);

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            if (!WaitForSecondsMap.ContainsKey(seconds))
            {
                WaitForSecondsMap.Add(seconds, new WaitForSeconds(seconds));
            }
            return WaitForSecondsMap[seconds];
        }
        
        public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
        {
            if (!WaitForSecondsRealtimeMap.ContainsKey(seconds))
            {
                WaitForSecondsRealtimeMap.Add(seconds, new WaitForSecondsRealtime(seconds));
            }
            return WaitForSecondsRealtimeMap[seconds];
        }
        
        public class WaitUntilYield : CustomYieldInstruction
        {
            public override bool keepWaiting => !_callback.Invoke();
            
            private readonly Func<bool> _callback;

            public WaitUntilYield(Func<bool> callback)
            {
                _callback = callback;
            }
        }

        public class WaitUntilAllYield : CustomYieldInstruction
        {
            public override bool keepWaiting => !_callbacks.TrueForAll(callback => callback());
            
            private readonly List<Func<bool>> _callbacks;

            public WaitUntilAllYield(List<Func<bool>> callbacks)
            {
                _callbacks = callbacks;
            }
        }
        
        public class WaitUntilOneOfYield : CustomYieldInstruction
        {
            public override bool keepWaiting => _callbacks.All(callback => !callback());
            
            private readonly List<Func<bool>> _callbacks;

            public WaitUntilOneOfYield(List<Func<bool>> callbacks)
            {
                _callbacks = callbacks;
            }
        }
    }
}