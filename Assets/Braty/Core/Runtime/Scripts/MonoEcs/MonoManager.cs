using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public static class MonoManager
    {
        private static readonly Dictionary<Type, object> _units = new();
        private static readonly Dictionary<Type, MonoSingleton> _singletons = new();
        public static readonly Dictionary<Type, MonoSystem> _monoSystems = new();
        
        public static void Init()
        {
            _units.Clear();
            _singletons.Clear();
            _monoSystems.Clear();
        }
        
        public static T GetSystem<T>() where T : MonoSystem
        {
            var systemType = typeof(T);
            return (T)_monoSystems[systemType];
        }
        
        internal static void AddSystem<T>(T monoSystem) where T : MonoSystem
        {
            Type systemKey = typeof(MonoSystem);
            if (_monoSystems.ContainsKey(systemKey))
            {
                Debug.LogError($"system {systemKey} is already registered");
            }

            _monoSystems[systemKey] = monoSystem;
        }
        
        internal static void RemoveSystem<T>(T monoSystem) where T : MonoSystem
        {
            Type systemKey = typeof(MonoSystem);
            if (!_monoSystems.ContainsKey(systemKey))
            {
                Debug.LogError($"system {systemKey} is not registered");
                return;
            }

            _monoSystems.Remove(systemKey);
        }
        
        public static T GetSingleton<T>() where T : MonoSingleton
        {
            var singletonType = typeof(T);
            return (T)_singletons[singletonType];
        }

        internal static void AddSingleton<T>(T singleton) where T : MonoSingleton
        {
            var singletonType = typeof(T);
            if (_singletons.ContainsKey(singletonType))
            {
                Debug.LogError($"Singleton {singletonType} is already registered");
            }
            _singletons[singletonType] = singleton;
        }

        internal static void RemoveSingleton<T>(T singleton) where T : MonoSingleton
        {
            var singletonType = typeof(T);
            if (!_singletons.ContainsKey(singletonType))
            {
                Debug.LogError($"Singleton {singletonType} is not registered already");
                return;
            }

            _singletons.Remove(singletonType);
        }

        
        public static IReadOnlyList<T> GetUnits<T>() where T : MonoUnit
        {
            var monoType = typeof(T);
            if (!_units.ContainsKey(monoType))
            {
                _units.TryAdd(monoType, new List<T>());
            }

            return (IReadOnlyList<T>)_units[monoType];
        }
        
        internal static void AddUnit<T>(T monoInstance) where T : MonoUnit
        {
            var monoType = typeof(T);
            if (!_units.ContainsKey(monoType))
            {
                _units.TryAdd(monoType, new List<T>());
            }
            
            ((List<T>)_units[monoType]).Add(monoInstance);
        }
        
        internal static void RemoveUnit<T>(T monoInstance) where T : MonoUnit
        {
            var monoType = typeof(T);
            if (!_units.ContainsKey(monoType))
            {
                Debug.LogError($"Mono list {monoType} is empty");
                return;
            }

            var monoList = (List<T>)_units[monoType];
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