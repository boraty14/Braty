using System;

namespace Braty.Core.Runtime.Scripts.Signals
{
    public interface ISignalBus
    {
        void Register<T>(Action<T> action) where T : ISignal;
        void Unregister<T>(Action<T> action) where T : ISignal;
        void Invoke<T>(T arg) where T : ISignal;
    }
}