using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(SpriteMask))]
    public class BSpriteMask : BElement
    {
        private SpriteMask _spriteMask;
        public SpriteMask SpriteMask => _spriteMask;
        
        private void Awake()
        {
            _spriteMask = GetComponent<SpriteMask>();
        }

        protected override void OnPriorityChanged(int priority)
        {
            base.OnPriorityChanged(priority);
            _spriteMask.isCustomRangeActive = true;
            _spriteMask.frontSortingOrder = priority;
            _spriteMask.backSortingOrder = priority - 1;
            _spriteMask.sortingOrder = priority;
        }
    }
}