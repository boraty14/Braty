using Braty.Core.Runtime.Scripts.BUI;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BHoverSprite : BInteractable
    {
        [SerializeField] [Range(0f, 1f)] private float _hoverAlpha = 0.75f; 
        [SerializeField] [Range(0f, 1f)] private float _normalAlpha = 1f; 
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void MouseEnterEvent(Vector2 mousePosition)
        {
            base.MouseEnterEvent(mousePosition);
            var color = _spriteRenderer.color;
            color.a = _hoverAlpha;
            _spriteRenderer.color = color;
        }

        public override void MouseExitEvent(Vector2 mousePosition)
        {
            base.MouseExitEvent(mousePosition);
            var color = _spriteRenderer.color;
            color.a = _normalAlpha;
            _spriteRenderer.color = color;
            
        }
    }
}