using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        private void OnEnable()
        {
            MonoManager.AddSingleton(this);            
        }

        private void OnDisable()
        {
            MonoManager.RemoveSingleton(this);            
        }
    }
}