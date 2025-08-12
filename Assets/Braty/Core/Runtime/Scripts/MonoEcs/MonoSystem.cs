using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoSystem : MonoBehaviour
    {
        protected virtual void Awake()
        {
            MonoManager.AddSystem(this);            
        }

        protected virtual void OnDestroy()
        {
            MonoManager.RemoveSystem(this);            
        }
    }
}