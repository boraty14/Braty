using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class BScrollListView<TScrollListItem, TScrollListItemView> : BInteractable
        where TScrollListItem : BScrollListItem
        where TScrollListItemView : BScrollListItemView<TScrollListItem>
    {
        [Header("Scroll References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteMask _spriteMask;

        [Header("Scroll Settings")]
        [SerializeField] private float _size;
        [SerializeField] private float _otherAxisSize;
        [SerializeField] private BScrollDirection _scrollDirection;
        [SerializeField] private TScrollListItemView _scrollListItemViewPrefab;
        [SerializeField] private float _startPadding;
        [SerializeField] private float _endPadding;

        [Header("Pool Settings")]
        [SerializeField] private bool _collectionCheck;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxCapacity = 100;

        public float Size => _size;
        public BScrollDirection ScrollDirection => _scrollDirection;

        private Vector2 _lastMousePosition;
        private ObjectPool<TScrollListItemView> _scrollListItemViewPool;
        private List<TScrollListItem> _items;
        private List<TScrollListItemView> _activeItemViews;
        private float _currentScrollDelta;

        private bool _isMouseDown;
        private bool _isInputBlocked;

        private float AbstractSize => _startPadding + _endPadding + GetAllItemsTotalSize();
        private float AbstractScrollableSize => AbstractSize - _size;

        public void AddItem(TScrollListItem item)
        {
            _items.Add(item);
            RenderScrollList();
        }

        public void RemoveItem(TScrollListItem item)
        {
            _items.Remove(item);
            RenderScrollList();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _items.Count) return;
            _items.RemoveAt(index);
            RenderScrollList();
        }

        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index + count > _items.Count) return;
            _items.RemoveRange(index, count);
            RenderScrollList();
        }

        public void ClearItems()
        {
            _items.Clear();
            RenderScrollList();
        }

        public void ScrollToElement(int index, float viewRatio)
        {
            index = Mathf.Clamp(index, 0, _items.Count - 1);
            float totalSize = _startPadding;
            for (int i = 0; i < index; i++)
            {
                totalSize += _items[i].Size;
            }

            float scrollRatio = totalSize / AbstractScrollableSize;
            ScrollTo(scrollRatio, viewRatio);
        }

        public void ScrollTo(float scrollRatio, float viewRatio)
        {
            scrollRatio = Mathf.Clamp01(scrollRatio);
            viewRatio = Mathf.Clamp01(viewRatio);
            _currentScrollDelta = Mathf.Lerp(0f, AbstractScrollableSize, scrollRatio);
            _currentScrollDelta += Mathf.Lerp(0f, _size, viewRatio);
            RenderScrollList();
        }

        public async UniTask ScrollToElementWithDuration(int index, float viewRatio, float duration,
            Ease ease = Ease.Linear)
        {
            index = Mathf.Clamp(index, 0, _items.Count - 1);
            float totalSize = _startPadding;
            for (int i = 0; i < index; i++)
            {
                totalSize += _items[i].Size;
            }

            float scrollRatio = totalSize / AbstractScrollableSize;
            await ScrollToWithDuration(scrollRatio, viewRatio, duration, ease);
        }

        public async UniTask ScrollToWithDuration(float scrollRatio, float viewRatio, float duration,
            Ease ease = Ease.Linear)
        {
            scrollRatio = Mathf.Clamp01(scrollRatio);
            viewRatio = Mathf.Clamp01(viewRatio);
            _isInputBlocked = true;
            float startScrollDelta = _currentScrollDelta;
            float targetScrollDelta = (scrollRatio * AbstractScrollableSize) + Mathf.Lerp(0f, _size, viewRatio);
            await Tween.Custom(startScrollDelta, targetScrollDelta, duration, newScrollDelta =>
                {
                    _currentScrollDelta = newScrollDelta;
                    RenderScrollList();
                }, ease: ease).ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy())
                .SuppressCancellationThrow();
        }

        private void Awake()
        {
            _activeItemViews = new();
            _items = new();
            _currentScrollDelta = 0f;
            _scrollListItemViewPool = new ObjectPool<TScrollListItemView>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                _collectionCheck,
                _defaultCapacity,
                _maxCapacity);
            SetReferences();
            RenderScrollList();
        }

        private void SetReferences()
        {
            _spriteRenderer.drawMode = SpriteDrawMode.Sliced;

            var boxCollider = GetComponent<BoxCollider2D>();
            switch (ScrollDirection)
            {
                case BScrollDirection.Horizontal:
                    _spriteRenderer.transform.localPosition =
                        new Vector3(Size * 0.5f, 0f, _spriteRenderer.transform.localPosition.z);
                    boxCollider.offset = new Vector2(Size * 0.5f, 0f);
                    _spriteRenderer.size = new Vector2(Size, _otherAxisSize);
                    boxCollider.size = new Vector2(Size, _otherAxisSize);
                    break;
                case BScrollDirection.Vertical:
                    _spriteRenderer.transform.localPosition =
                        new Vector3(0f, Size * 0.5f, _spriteRenderer.transform.localPosition.z);
                    boxCollider.offset = new Vector2(0f, Size * 0.5f);
                    _spriteRenderer.size = new Vector2(_otherAxisSize, Size);
                    boxCollider.size = new Vector2(_otherAxisSize, Size);
                    break;
            }
        }

        private void Update()
        {
            if (_isMouseDown) return;
            // braty todo inertia
        }

        public override void MouseDownEvent(Vector2 mousePosition)
        {
            base.MouseDownEvent(mousePosition);
            _lastMousePosition = mousePosition;
            _isMouseDown = true;
        }

        public override void MouseUpEvent(Vector2 mousePosition)
        {
            base.MouseUpEvent(mousePosition);
            _isMouseDown = false;
        }

        public override void MouseExitEvent(Vector2 mousePosition)
        {
            base.MouseExitEvent(mousePosition);
            _isMouseDown = false;
        }

        public override void MouseDragEvent(Vector2 mousePosition)
        {
            base.MouseDragEvent(mousePosition);
            if (_isInputBlocked || !_isMouseDown) return;

            Vector2 deltaVector = _lastMousePosition - mousePosition;
            float deltaAmount = 0f;
            switch (_scrollDirection)
            {
                case BScrollDirection.Horizontal:
                    deltaAmount = deltaVector.x;
                    break;
                case BScrollDirection.Vertical:
                    deltaAmount = deltaVector.y;
                    break;
            }

            if (_currentScrollDelta + deltaAmount <= 0f)
            {
                deltaAmount = -_currentScrollDelta;
            }
            else if (_currentScrollDelta + deltaAmount >= AbstractSize - _size)
            {
                deltaAmount = AbstractSize - _size - _currentScrollDelta;
            }

            _currentScrollDelta += deltaAmount;
            RenderScrollList();

            _lastMousePosition = mousePosition;
        }

        private void RenderScrollList()
        {
            // clear if no elements
            if (_items.Count == 0)
            {
                ReleaseItemViews(0);
                return;
            }

            // get starting item index
            float currentItemViewEndPosition = _startPadding;
            int currentItemIndex = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                currentItemIndex = i;
                currentItemViewEndPosition += _items[currentItemIndex].Size;
                if (currentItemViewEndPosition >= _currentScrollDelta) break;
            }


            // render first element
            var firstItemView = GetItemView(0);
            var firstItem = _items[currentItemIndex];
            firstItemView.Render(firstItem,
                GetItemViewPosition((currentItemViewEndPosition - _currentScrollDelta) - firstItem.Size * 0.5f));

            // render rest
            int itemViewCount = 1;
            while (currentItemViewEndPosition < _currentScrollDelta + _size && currentItemIndex < _items.Count - 1)
            {
                currentItemIndex++;
                itemViewCount++;
                currentItemViewEndPosition += _items[currentItemIndex].Size;
                var currentItemView = GetItemView(itemViewCount - 1);
                var currentItem = _items[currentItemIndex];
                currentItemView.Render(currentItem,
                    GetItemViewPosition((currentItemViewEndPosition - _currentScrollDelta) - currentItem.Size * 0.5f));
            }

            ReleaseItemViews(itemViewCount);
        }

        private TScrollListItemView GetItemView(int index)
        {
            if (index < _activeItemViews.Count) return _activeItemViews[index];
            var scrollListItemView = _scrollListItemViewPool.Get();
            _activeItemViews.Add(scrollListItemView);
            return scrollListItemView;
        }

        private Vector2 GetItemViewPosition(float position)
        {
            return _scrollDirection switch
            {
                BScrollDirection.Horizontal => new Vector2(position, 0),
                BScrollDirection.Vertical => new Vector2(0, position),
                _ => Vector2.zero
            };
        }

        private void ReleaseItemViews(int itemCount)
        {
            if (itemCount == _activeItemViews.Count) return;
            for (int i = itemCount; i < _activeItemViews.Count; i++)
            {
                _scrollListItemViewPool.Release(_activeItemViews[i]);
            }

            _activeItemViews.RemoveRange(itemCount, _activeItemViews.Count - itemCount);
        }

        private float GetAllItemsTotalSize()
        {
            float total = 0f;
            foreach (var item in _items)
            {
                total += item.Size;
            }

            return total;
        }

        private TScrollListItemView CreateSetup() => Instantiate(_scrollListItemViewPrefab, transform);
        private void GetSetup(TScrollListItemView obj) => obj.gameObject.SetActive(true);
        private void ReleaseSetup(TScrollListItemView obj) => obj.gameObject.SetActive(false);
        private void DestroySetup(TScrollListItemView obj) => Destroy(obj);

        private void OnDrawGizmosSelected()
        {
            Vector3 scrollViewOffset = Vector3.zero;
            switch (_scrollDirection)
            {
                case BScrollDirection.Horizontal:
                    scrollViewOffset = Vector3.right * _size * transform.localScale.x;
                    break;
                case BScrollDirection.Vertical:
                    scrollViewOffset = Vector3.up * _size * transform.localScale.y;
                    break;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + scrollViewOffset);
        }
    }
}