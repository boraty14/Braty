using TMPro;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(TMP_Text))]
    [RequireComponent(typeof(MeshRenderer))]
    public class BText : BElement
    {
        private TMP_Text _text;
        private MeshRenderer _meshRenderer;

        public TMP_Text Text => _text;
        public MeshRenderer MeshRenderer => _meshRenderer;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        protected override void OnPriorityChanged(int priority)
        {
            base.OnPriorityChanged(priority);
            _meshRenderer.sortingOrder = priority;
        }
    }
}