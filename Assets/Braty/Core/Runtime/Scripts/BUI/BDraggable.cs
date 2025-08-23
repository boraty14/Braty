using Braty.Core.Runtime.Scripts.BUI;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Utils
{
    public class BDraggable : BInteractable
    {
        private Vector2 _lastMousePosition;

        public override void MouseDownEvent(Vector2 mousePosition)
        {
            base.MouseDownEvent(mousePosition);
            _lastMousePosition = mousePosition;
        }

        public override void MouseDragEvent(Vector2 mousePosition)
        {
            base.MouseDragEvent(mousePosition);
            var delta = mousePosition - _lastMousePosition;
            transform.localPosition += new Vector3(delta.x, delta.y, 0f);
            _lastMousePosition = mousePosition;
        }
    }
}