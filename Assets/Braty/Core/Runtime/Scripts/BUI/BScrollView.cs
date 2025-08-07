using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public class BScrollView<TScrollItem, TScrollItemView> : BInteractable where TScrollItem : BScrollItem
        where TScrollItemView : BScrollItemView<TScrollItem>
    {
        [SerializeField] private BScrollDirection _scrollDirection = BScrollDirection.Vertical;
        [SerializeField] private SpriteMask _spriteMask;

        public override void SetPriority(int newPriority)
        {
            base.SetPriority(newPriority);
            _spriteMask.frontSortingOrder = Priority;
            _spriteMask.backSortingOrder = Priority - 1;
            _spriteMask.frontSortingLayerID = SortingLayer.NameToID(BConstants.UISortingLayerName);
            _spriteMask.backSortingLayerID = SortingLayer.NameToID(BConstants.UISortingLayerName);
        }

        //[SerializeField]
    }
}