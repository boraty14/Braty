using System;
using UnityEngine;

namespace Braty.Core.Runtime.MonoEvents
{
    public class MonoEventDispatcher : MonoBehaviour, IMonoEventDispatcher
    {
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

        private void Start()
        {
            OnStart?.Invoke();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                OnAppPause?.Invoke();
            }

            else
            {
                OnAppResume?.Invoke();
            }
        }
    }
}