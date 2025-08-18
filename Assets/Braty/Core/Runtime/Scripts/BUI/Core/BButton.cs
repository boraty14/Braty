using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    public class BButton : BInteractable
    {
        [Header("Button Settings")]
        [SerializeField] private float _clickDeltaThreshold = 0.1f;
        [SerializeField] private float _clickDurationThreshold = 0.5f;
        [SerializeField] private SpriteRenderer _buttonRenderer;

        [Header("Animation Settings")]
        public bool IsAnimationEnabled;
        public Vector2 AnimationScale = new Vector2(0.9f, 0.9f);

        private float _mouseDownTime = 0f;
        private bool _isMouseDown = false;
        private Vector2 _mouseDownPosition;

        public float ClickDeltaThreshold => _clickDeltaThreshold;
        public float ClickDurationThreshold => _clickDurationThreshold;
        public SpriteRenderer ButtonRenderer => _buttonRenderer;

        public event Action OnClick;


        public override void MouseDownEvent(Vector2 mousePosition)
        {
            base.MouseDownEvent(mousePosition);
            _mouseDownPosition = mousePosition;
            _mouseDownTime = Time.time;
            EnterClickState();
        }

        public override void MouseUpEvent(Vector2 mousePosition)
        {
            base.MouseUpEvent(mousePosition);

            if (!_isMouseDown) return;
            ExitClickState();

            if (Time.time - _mouseDownTime > ClickDurationThreshold ||
                Vector2.Distance(_mouseDownPosition, mousePosition) > ClickDeltaThreshold) return;

            OnClick?.Invoke();
        }

        public override void MouseDragEvent(Vector2 mousePosition)
        {
            base.MouseDragEvent(mousePosition);
            if (!_isMouseDown) return;
            if (Vector2.Distance(_mouseDownPosition, mousePosition) <= ClickDeltaThreshold) return;
            ExitClickState();
        }

        public override void MouseExitEvent(Vector2 mousePosition)
        {
            base.MouseExitEvent(mousePosition);
            if (!_isMouseDown) return;
            ExitClickState();
        }

        private void EnterClickState()
        {
            _isMouseDown = true;
            if (IsAnimationEnabled && ButtonRenderer != null)
            {
                ButtonRenderer.transform.localScale = new Vector3(AnimationScale.x, AnimationScale.y, 1);
            }
        }

        private void ExitClickState()
        {
            _isMouseDown = false;
            if (ButtonRenderer != null)
            {
                ButtonRenderer.transform.localScale = Vector3.one;
            }
        }
    }
}