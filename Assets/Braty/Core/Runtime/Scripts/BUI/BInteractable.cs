using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class BInteractable : MonoBehaviour
    {
        public int Priority;

        public virtual void MouseDownEvent()
        {
            
        }
        
        public virtual void MouseDragEvent()
        {
            
        }
        
        public virtual void MouseEnterEvent(Vector2 mousePosition)
        {
            
        }
        
        public virtual void MouseExitEvent(Vector2 mousePosition)
        {
            
        }
        
        public virtual void MouseOverEvent(Vector2 mousePosition)
        {
               
        }

        public virtual void MouseUpEvent()
        {
            
        }
    }
}