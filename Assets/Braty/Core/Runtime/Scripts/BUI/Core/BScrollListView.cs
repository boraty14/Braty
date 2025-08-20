using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    public abstract class BScrollListView<TScrollListItem, TScrollListItemView> : BInteractable
        where TScrollListItem : BScrollListItem
        where TScrollListItemView : BScrollListItemView<TScrollListItem>

    {
        [Header("Scroll Settings")]
        public float Size;
        [SerializeField] private BScrollDirection _scrollDirection;
        [SerializeField] private TScrollListItemView _scrollListItemViewPrefab;
        [SerializeField] private float _startPadding;
        [SerializeField] private float _endPadding;
        [SerializeField] private float _itemSize;

        [Header("Pool Settings")]
        [SerializeField] private bool _collectionCheck;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxCapacity = 100;
        
        private Vector2 _lastMousePosition;
        private bool _isMouseDown;
        private BScrollDirection _initScrollDirection;
        private ObjectPool<TScrollListItemView> _scrollListItemViewPool;
        private List<TScrollListItemView> _activeItems;
        private float _currentScrollDelta;

        private void Awake()
        {
            _initScrollDirection = _scrollDirection;
            _activeItems = new();
            _currentScrollDelta = 0f;
            _scrollListItemViewPool = new ObjectPool<TScrollListItemView>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                _collectionCheck,
                _defaultCapacity,
                _maxCapacity);
        }

        public override void MouseDragEvent(Vector2 mousePosition)
        {
            base.MouseDragEvent(mousePosition);
            var delta = mousePosition - _lastMousePosition;
            switch (_initScrollDirection)
            {
                case BScrollDirection.Horizontal:
                    _currentScrollDelta += delta.x;
                    break;
                case BScrollDirection.Vertical:
                    _currentScrollDelta += delta.y;
                    break;
            }
            TranslateScrollArea(delta);
            _lastMousePosition = mousePosition;
        }

        private void TranslateScrollArea(Vector2 delta)
        {
            switch (_initScrollDirection)
            {
                case BScrollDirection.Horizontal:
                    //_scrollArea.Move(Vector3.right * delta.x);
                    break;
                case BScrollDirection.Vertical:
                    //_scrollArea.Move(Vector3.up * delta.y);
                    break;
            }
        }

        private void Move(Vector3 delta)
        {
            var newPosition = transform.localPosition + delta;
            newPosition.x = Mathf.Clamp(newPosition.x, -Size * 0.5f, Size * 0.5f);
            newPosition.y = Mathf.Clamp(newPosition.y, -Size * 0.5f, Size * 0.5f);
            transform.localPosition = newPosition;
        }

        private void Render()
        {
            
        }

        private TScrollListItemView CreateSetup() => Instantiate(_scrollListItemViewPrefab);
        private void GetSetup(TScrollListItemView obj) => obj.gameObject.SetActive(true);
        private void ReleaseSetup(TScrollListItemView obj) => obj.gameObject.SetActive(false);
        private void DestroySetup(TScrollListItemView obj) => Destroy(obj);

        protected virtual TScrollListItemView Get()
        {
            var scrollListItemView = _scrollListItemViewPool.Get();
            _activeItems.Add(scrollListItemView);
            return scrollListItemView;
        }

        protected virtual void Release(TScrollListItemView scrollListItemView)
        {
            _activeItems.Remove(scrollListItemView);
            _scrollListItemViewPool.Release(scrollListItemView);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 scrollAreaOffset = Vector3.zero;
            Vector3 scrollViewOffset = Vector3.zero;
            switch (_scrollDirection)
            {
                case BScrollDirection.Horizontal:
                    scrollViewOffset = Vector3.right * Size * 0.5f;
                    break;
                case BScrollDirection.Vertical:
                    scrollViewOffset = Vector3.up * Size * 0.5f;
                    break;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - scrollViewOffset, transform.position + scrollViewOffset);
        }
    }
}