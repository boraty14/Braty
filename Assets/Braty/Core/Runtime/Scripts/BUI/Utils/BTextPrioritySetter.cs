using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Utils
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BTextPrioritySetter : BPrioritySetter
    {
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        protected override void OnPriorityChanged(int priority)
        {
            _meshRenderer.sortingOrder = priority + Offset;
        }
    }
}