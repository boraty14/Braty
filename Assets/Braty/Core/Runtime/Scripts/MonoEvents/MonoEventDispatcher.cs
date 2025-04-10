using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEvents
{
    public class MonoEventDispatcher : IMonoEventDispatcher
    {
        private readonly MonoEventDispatcherBehaviour _monoEventDispatcherBehaviour;

        public MonoEventDispatcher()
        {
            _monoEventDispatcherBehaviour = new GameObject("MonoEventDispatcherBehaviour").AddComponent<MonoEventDispatcherBehaviour>();
            _monoEventDispatcherBehaviour.OnStart += () => OnStart?.Invoke();
            _monoEventDispatcherBehaviour.OnUpdate += () => OnUpdate?.Invoke();
            _monoEventDispatcherBehaviour.OnFixedUpdate += () => OnFixedUpdate?.Invoke();
            _monoEventDispatcherBehaviour.OnLateUpdate += () => OnLateUpdate?.Invoke();
            _monoEventDispatcherBehaviour.OnAppPause += () => OnAppPause?.Invoke();
            _monoEventDispatcherBehaviour.OnAppResume += () => OnAppResume?.Invoke();
        }
        
        private event Action OnAppPause;
        event Action IMonoEventDispatcher.OnAppPause
        {
            add => this.OnAppPause += value;
            remove => this.OnAppPause -= value;
        }
        private event Action OnAppResume;
        event Action IMonoEventDispatcher.OnAppResume
        {
            add => this.OnAppResume += value;
            remove => this.OnAppResume -= value;
        }
        private event Action OnStart;
        event Action IMonoEventDispatcher.OnStart
        {
            add => this.OnStart += value;
            remove => this.OnStart -= value;
        }
        private event Action OnUpdate;
        event Action IMonoEventDispatcher.OnUpdate
        {
            add => this.OnUpdate += value;
            remove => this.OnUpdate -= value;
        }
        private event Action OnFixedUpdate;
        event Action IMonoEventDispatcher.OnFixedUpdate
        {
            add => this.OnFixedUpdate += value;
            remove => this.OnFixedUpdate -= value;
        }
        private event Action OnLateUpdate;
        event Action IMonoEventDispatcher.OnLateUpdate
        {
            add => this.OnLateUpdate += value;
            remove => this.OnLateUpdate -= value;
        }
    }
}