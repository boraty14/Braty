using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoSystem : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            MonoSystemManager.AddSystem(this);            
        }

        protected virtual void OnDisable()
        {
            MonoSystemManager.RemoveSystem(this);            
        }
    }
}