using Braty.Core.Runtime.Scripts.BUI.Core;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Utils
{
    public abstract class BPrioritySetter : MonoBehaviour
    {
        [SerializeField] private BInteractable _bInteractable;
        [SerializeField] private int _offset;

        public int Offset => _offset;

        private void OnEnable()
        {
            _bInteractable.OnPriorityChanged += OnPriorityChanged;
        }

        private void OnDisable()
        {
            _bInteractable.OnPriorityChanged -= OnPriorityChanged;
        }

        protected abstract void OnPriorityChanged(int priority);
    }
}