using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public static class MonoManager
    {
        private static readonly Dictionary<Type, object> _activeMonos = new();
        
        public static void Init() => _activeMonos.Clear();

        public static IReadOnlyList<T> GetActiveMonos<T>() where T : MonoUnit
        {
            var monoType = typeof(T);
            if (!_activeMonos.ContainsKey(monoType))
            {
                _activeMonos.TryAdd(monoType, new List<T>());
            }

            return (IReadOnlyList<T>)_activeMonos[monoType];
        }
        
        internal static void AddActiveMono<T>(T monoInstance) where T : MonoUnit
        {
            var monoType = typeof(T);
            if (!_activeMonos.ContainsKey(monoType))
            {
                _activeMonos.TryAdd(monoType, new List<T>());
            }
            
            ((List<T>)_activeMonos[monoType]).Add(monoInstance);
        }
        
        internal static void RemoveActiveMono<T>(T monoInstance) where T : MonoUnit
        {
            var monoType = typeof(T);
            if (!_activeMonos.ContainsKey(monoType))
            {
                Debug.LogError($"Mono list {monoType} is empty");
                return;
            }

            var monoList = (List<T>)_activeMonos[monoType];
            int monoIndex = monoList.IndexOf(monoInstance);
            if (monoIndex < 0)
            {
                Debug.LogError($"Mono is not in list {monoType}");
                return;
            }

            monoList[monoIndex] = monoList[^1];
            monoList.RemoveAt(monoList.Count - 1);
        }
        
    }
}