using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BSpritePrioritySetter : BPrioritySetter
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnPriorityChanged(int priority)
        {
            _spriteRenderer.sortingOrder = priority + Offset;
        }
    }
}