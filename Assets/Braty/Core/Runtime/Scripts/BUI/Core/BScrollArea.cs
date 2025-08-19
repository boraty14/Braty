using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    [DisallowMultipleComponent]
    public class BScrollArea : MonoBehaviour
    {
        public float Size;
        public Vector3 ScrollPosition => transform.localPosition;

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        public void Move(Vector3 delta)
        {
            var newPosition = transform.localPosition + delta;
            newPosition.x = Mathf.Clamp(newPosition.x, -Size * 0.5f, Size * 0.5f);
            newPosition.y = Mathf.Clamp(newPosition.y, -Size * 0.5f, Size * 0.5f);
            transform.localPosition = newPosition;
        }
    }
}