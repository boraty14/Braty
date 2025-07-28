using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoUnit : MonoBehaviour
    {
        private void OnEnable()
        {
            MonoManager.AddActiveMono(this);
        }

        private void OnDisable()
        {
            MonoManager.RemoveActiveMono(this);
        }
    }
}