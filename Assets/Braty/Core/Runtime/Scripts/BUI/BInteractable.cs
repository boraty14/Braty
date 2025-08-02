using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class BInteractable : MonoBehaviour
    {
        public int Priority;

        public void MouseDownEvent()
        {
            
        }
        
        public void MouseDragEvent()
        {
            
        }
        
        public void MouseEnterEvent()
        {
            
        }
        
        public void MouseExitEvent()
        {
            
        }
        
        public void MouseOverEvent()
        {
            
        }

        public void MouseUpEvent()
        {
            
        }
    }
}