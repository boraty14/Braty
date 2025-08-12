using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoUnit : MonoBehaviour
    {
        public bool IsActive;
        
        private void Awake()
        {
            MonoManager.AddUnit(this);
        }

        private void OnDestroy()
        {
            MonoManager.RemoveUnit(this);
        }
    }
}