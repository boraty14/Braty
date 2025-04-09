using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.State
{
    public class MonoStateManager : MonoBehaviour, IMonoStateManager
    {
        private readonly Dictionary<Type, MonoState> _states = new();
        
        private void Awake()
        {
            var states = GetComponents<MonoState>();
            foreach (var state in states)
            {
                _states.TryAdd(state.GetType(), state);
            }
        }

        T IMonoStateManager.Get<T>()
        {
            return (T)_states[typeof(T)];
        }
    }
}