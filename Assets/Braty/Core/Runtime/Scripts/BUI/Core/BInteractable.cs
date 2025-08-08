using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    [RequireComponent(typeof(Collider2D))]
    [DisallowMultipleComponent]
    public abstract class BInteractable : MonoBehaviour
    {
        [SerializeField] private int _priority;

        public event Action<Vector2> OnMouseDownEvent;
        public event Action<Vector2> OnMouseDragEvent;
        public event Action<Vector2> OnMouseEnterEvent;
        public event Action<Vector2> OnMouseExitEvent;
        public event Action<Vector2> OnMouseOverEvent;
        public event Action<Vector2> OnMouseUpEvent;
        public event Action<Vector2> OnRightClickEvent;
        public event Action<int> OnPriorityChanged; 

        public int Priority => _priority;

        public virtual void Start()
        {
            SetPriority(Priority);
        }

        public void SetPriority(int newPriority)
        {
            _priority = newPriority;
            OnPriorityChanged?.Invoke(_priority);
        }
        
        public virtual void MouseDownEvent(Vector2 mousePosition)
        {
            OnMouseDownEvent?.Invoke(mousePosition);
        }

        public virtual void MouseDragEvent(Vector2 mousePosition)
        {
            OnMouseDragEvent?.Invoke(mousePosition);
        }

        public virtual void MouseEnterEvent(Vector2 mousePosition)
        {
            OnMouseEnterEvent?.Invoke(mousePosition);
        }

        public virtual void MouseExitEvent(Vector2 mousePosition)
        {
            OnMouseExitEvent?.Invoke(mousePosition);
        }

        public virtual void MouseOverEvent(Vector2 mousePosition)
        {
            OnMouseOverEvent?.Invoke(mousePosition);
        }

        public virtual void MouseUpEvent(Vector2 mousePosition)
        {
            OnMouseUpEvent?.Invoke(mousePosition);
        }
        
        public virtual void RightClickEvent(Vector2 mousePosition)
        {
            OnRightClickEvent?.Invoke(mousePosition);
        }
    }
}