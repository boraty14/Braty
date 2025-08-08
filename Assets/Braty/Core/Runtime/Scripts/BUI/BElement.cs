using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [DisallowMultipleComponent]
    public abstract class BElement : MonoBehaviour
    {
        private BInteractable _bInteractable;
        
        private void OnEnable()
        {
            if (_bInteractable == null)
            {
                _bInteractable = GetComponentInParent<BInteractable>();
            }

            _bInteractable.OnPriorityChanged += OnPriorityChanged;
            OnPriorityChanged(_bInteractable.Priority);
        }

        private void OnDisable()
        {
            if (_bInteractable == null)
            {
                return;
            }
            _bInteractable.OnPriorityChanged -= OnPriorityChanged;
        }

        protected virtual void OnPriorityChanged(int priority)
        {
            
        }
    }
}