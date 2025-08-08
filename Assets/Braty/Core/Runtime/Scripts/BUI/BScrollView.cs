using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(SpriteMask))]
    public class BScrollView<TScrollItem, TScrollItemView> : BInteractable where TScrollItem : BScrollItem
        where TScrollItemView : BScrollItemView<TScrollItem>
    {
        [SerializeField] private BScrollDirection _scrollDirection = BScrollDirection.Vertical;
        [SerializeField] private SpriteMask _spriteMask;

        public SpriteMask SpriteMask => _spriteMask;
        public BScrollDirection ScrollDirection => _scrollDirection;
    }
}