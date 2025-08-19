using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    public class BScrollView : BInteractable
    {
        public float Size;
        [SerializeField] private BScrollArea _scrollArea;
        [SerializeField] private BScrollDirection _scrollDirection;
        
        private Vector2 _lastMousePosition;
        private bool _isMouseDown;
        private BScrollDirection _initScrollDirection;

        private void Awake()
        {
            _initScrollDirection = _scrollDirection;
        }

        public override void MouseDownEvent(Vector2 mousePosition)
        {
            base.MouseDownEvent(mousePosition);
            _lastMousePosition = mousePosition;
        }

        public override void MouseDragEvent(Vector2 mousePosition)
        {
            base.MouseDragEvent(mousePosition);
            var delta = mousePosition - _lastMousePosition;
            switch (_initScrollDirection)
            {
                case BScrollDirection.Horizontal:
                    _scrollArea.Move(Vector3.right * delta.x);
                    break;
                case BScrollDirection.Vertical:
                    _scrollArea.Move(Vector3.up * delta.y);
                    break;
            }

            _lastMousePosition = mousePosition;
        }

        public override void MouseUpEvent(Vector2 mousePosition)
        {
            base.MouseUpEvent(mousePosition);
            if (!_isMouseDown) return;
            _isMouseDown = false;
        }
        
        public override void MouseExitEvent(Vector2 mousePosition)
        {
            base.MouseExitEvent(mousePosition);
            if (!_isMouseDown) return;
            _isMouseDown = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            Vector3 scrollAreaOffset = Vector3.zero;
            Vector3 scrollViewOffset = Vector3.zero;
            switch (_scrollDirection)
            {
                case BScrollDirection.Horizontal:
                    scrollAreaOffset = Vector3.right * _scrollArea.Size * 0.5f;
                    scrollViewOffset = Vector3.right * Size * 0.5f;
                    break;
                case BScrollDirection.Vertical:
                    scrollAreaOffset = Vector3.up * _scrollArea.Size * 0.5f;
                    scrollViewOffset = Vector3.up * Size * 0.5f;
                    break;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - scrollAreaOffset, transform.position + scrollAreaOffset);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - scrollViewOffset, transform.position + scrollViewOffset);
        }
    }
}