using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BSprite : BElement
    {
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnPriorityChanged(int priority)
        {
            base.OnPriorityChanged(priority);
            _spriteRenderer.sortingOrder = priority;
        }
    }
}