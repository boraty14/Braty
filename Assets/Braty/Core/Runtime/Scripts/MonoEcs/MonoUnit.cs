using UnityEngine;

namespace Braty.Core.Runtime.Scripts.MonoEcs
{
    public abstract class MonoUnit : MonoBehaviour
    {
        private void OnEnable()
        {
            MonoManager.AddUnit(this);
        }

        private void OnDisable()
        {
            MonoManager.RemoveUnit(this);
        }
    }
}