using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public class BButton : BInteractable
    {
        public float ClickThreshold = 0.5f;
        public SpriteRenderer ButtonRenderer;

        [Header("Animation Settings")]
        public bool IsAnimationEnabled;
        public Vector2 AnimaitonScale = new Vector2(0.9f,0.9f);


        private float _mouseDownTime = 0f;
        private bool _isMouseDown = false;

        public event Action OnClick;

        public override void SetPriority(int newPriority)
        {
            if (ButtonRenderer != null)
            {
                ButtonRenderer.sortingOrder = newPriority;
            }
            base.SetPriority(newPriority);
        }

        public override void MouseDownEvent(Vector2 mousePosition)
        {
            base.MouseDownEvent(mousePosition);
            _mouseDownTime = Time.time;
            _isMouseDown = true;
            if (IsAnimationEnabled && ButtonRenderer != null)
            {
                ButtonRenderer.transform.localScale = new Vector3(AnimaitonScale.x, AnimaitonScale.y, 1);
            }
        }

        public override void MouseUpEvent(Vector2 mousePosition)
        {
            base.MouseUpEvent(mousePosition);

            if (!_isMouseDown) return;
            _isMouseDown = false;

            if (ButtonRenderer != null)
            {
                ButtonRenderer.transform.localScale = Vector3.one;
            }

            if (Time.time - _mouseDownTime > ClickThreshold) return;

            OnClick?.Invoke();
        }

        public override void MouseExitEvent(Vector2 mousePosition)
        {
            base.MouseExitEvent(mousePosition);
            
            if (!_isMouseDown) return;
            _isMouseDown = false;
            
            if (ButtonRenderer != null)
            {
                ButtonRenderer.transform.localScale = Vector3.one;
            }
        }
    }
}