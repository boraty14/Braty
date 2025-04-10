using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.State
{
    public class MonoStateManager : IMonoStateManager
    {
        private readonly Dictionary<Type, MonoState> _states = new();
        private readonly MonoStateHolderBehaviour _monoStateHolderBehaviour;

        public MonoStateManager()
        {
            _monoStateHolderBehaviour = new GameObject("MonoStateHolderBehaviour").AddComponent<MonoStateHolderBehaviour>();
        }
        
        void IMonoStateManager.Add<T>()
        {
            var key = typeof(T);
            if (_states.ContainsKey(key))
            {
                Debug.LogError($"Key for mono state {key} already exists");
                return;
            }
            var stateBehaviour = _monoStateHolderBehaviour.gameObject.AddComponent<T>();
            _states.TryAdd(key, stateBehaviour);
        }

        T IMonoStateManager.Get<T>()
        {
            return (T)_states[typeof(T)];
        }
    }
}