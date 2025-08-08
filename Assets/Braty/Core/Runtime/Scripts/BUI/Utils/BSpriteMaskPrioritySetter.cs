using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI.Utils
{
    [RequireComponent(typeof(SpriteMask))]
    public class BSpriteMaskPrioritySetter : BPrioritySetter
    {
        private SpriteMask _spriteMask;

        private void Awake()
        {
            _spriteMask = GetComponent<SpriteMask>();
        }
        
        protected override void OnPriorityChanged(int priority)
        {
            int offsetPriority = priority + Offset;
            _spriteMask.isCustomRangeActive = true;
            _spriteMask.frontSortingOrder = offsetPriority;
            _spriteMask.backSortingOrder = offsetPriority - 1;
            _spriteMask.sortingOrder = offsetPriority;
        }
    }
}