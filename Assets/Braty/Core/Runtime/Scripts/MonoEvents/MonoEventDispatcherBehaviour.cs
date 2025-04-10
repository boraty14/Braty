using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEvents
{
    public class MonoEventDispatcherBehaviour : MonoBehaviour
    {
        public event Action OnStart;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;
        public event Action OnAppPause;
        public event Action OnAppResume;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
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