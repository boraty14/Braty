using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        private void Awake()
        {
            MonoManager.AddSingleton(this);            
        }

        private void OnDestroy()
        {
            MonoManager.RemoveSingleton(this);            
        }
    }
}