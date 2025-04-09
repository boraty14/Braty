using System;
using System.Collections.Generic;

namespace Braty.Core.Runtime.Signals
{
    public class SignalBus : ISignalBus
    {
        private readonly Dictionary<Type, object> _actions = new();

        void ISignalBus.Register<T>(Action<T> action)
        {
            var key = typeof(T);
            if (!_actions.TryGetValue(key, out var value))
            {
                _actions.TryAdd(key, action);
                return;
            }

            _actions[key] = (Action<T>)value + action;
        }

        void ISignalBus.Unregister<T>(Action<T> action)
        {
            var key = typeof(T);
            if (!_actions.TryGetValue(key, out var value))
            {
                return;
            }

            _actions[key] = (Action<T>)value - action;
        }


        void ISignalBus.Invoke<T>(T arg)
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