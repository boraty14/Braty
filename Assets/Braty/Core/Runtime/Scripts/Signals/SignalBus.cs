using System;
using System.Collections.Generic;

namespace Braty.Core.Runtime.Scripts.Signals
{
    public static class SignalBus
    {
        private static readonly Dictionary<Type, object> _actions = new();

        public static void Init() => _actions.Clear();

        public static void Register<T>(Action<T> action)
        {
            var key = typeof(T);
            if (!_actions.TryGetValue(key, out var value))
            {
                _actions.TryAdd(key, action);
                return;
            }

            _actions[key] = (Action<T>)value + action;
        }

        public static void Unregister<T>(Action<T> action)
        {
            var key = typeof(T);
            if (!_actions.TryGetValue(key, out var value))
            {
                return;
            }

            _actions[key] = (Action<T>)value - action;
        }


        public static void Invoke<T>(T arg)
        {
            var key = typeof(T);
            if (!_actions.TryGetValue(key, out var value))
            {
                return;
            }

            ((Action<T>)value)?.Invoke(arg);
        }
    }
}