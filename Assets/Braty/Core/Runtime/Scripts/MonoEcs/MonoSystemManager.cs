using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public static class MonoSystemManager
    {
        public static readonly Dictionary<Type, MonoSystem> _monoSystems = new();

        public static void Init()
        {
            _monoSystems.Clear();
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
    }
}