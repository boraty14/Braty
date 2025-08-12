using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Scene
{
    [DefaultExecutionOrder(-10)]
    public abstract class SceneInitializer : MonoBehaviour
    {
        private void Awake()
        {
            OnAwake();
        }

        protected abstract void OnAwake();
    }
}