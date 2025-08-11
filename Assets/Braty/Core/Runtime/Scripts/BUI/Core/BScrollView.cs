using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Scripts.BUI.Core
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BScrollView<TScrollItem, TScrollItemView> : BInteractable where TScrollItem : BScrollItem
        where TScrollItemView : BScrollItemView<TScrollItem>
    {
        [SerializeField] private BScrollDirection _scrollDirection = BScrollDirection.Vertical;
        [SerializeField] private SpriteMask _spriteMask;
        [SerializeField] private TScrollItemView _scrolItemViewPrefab;

        public SpriteMask SpriteMask => _spriteMask;
        public BScrollDirection ScrollDirection => _scrollDirection;

        private readonly List<TScrollItem> _scrollItems = new();
        private RectTransform _rectTransform;
        private ObjectPool<TScrollItemView> _scrollItemPool;

        private readonly List<TScrollItemView> _activeScrollItemViews = new();

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            InitPool();
        }

        protected override void Start()
        {
            base.Start();
            RefreshView();
        }

        public void GoToIndex(int index)
        {
            if (index < 0 || index >= _scrollItems.Count) return;
        }

        public void GoToItem(TScrollItem scrollItem)
        {
            int index = _scrollItems.IndexOf(scrollItem);
            if (index >= 0) GoToIndex(index);
        }

        public void AddItem(TScrollItem scrollItem)
        {
            _scrollItems.Add(scrollItem);
            RefreshView();
        }

        public void RemoveItems(TScrollItem scrollItem)
        {
            _scrollItems.Remove(scrollItem);
            RefreshView();
        }

        private void RefreshView()
        {
            int scrollItemCount = _scrollItems.Count;
            int preScrollItemViewCount = _activeScrollItemViews.Count;
            
            
            if (scrollItemCount > preScrollItemViewCount)
            {
                for (int i = 0; i < scrollItemCount - preScrollItemViewCount; i++)
                {
                    _activeScrollItemViews.Add(GetScrollItemView());
                }
            }
            else if (preScrollItemViewCount > scrollItemCount)
            {
                for (int i = 0; i < preScrollItemViewCount - scrollItemCount; i++)
                {
                    var lastActiveScrollItemView = _activeScrollItemViews[^1];
                    _activeScrollItemViews.RemoveAt(_activeScrollItemViews.Count - 1);
                    HideScrollItemView(lastActiveScrollItemView);
                }
            }

            for (int i = 0; i < scrollItemCount; i++)
            {
                TScrollItem scrollItem = _scrollItems[i];
            }
        }

        private TScrollItemView GetScrollItemView()
        {
            return _scrollItemPool.Get();
        }

        private void HideScrollItemView(TScrollItemView scrollItemView)
        {
            scrollItemView.Hide();
            _scrollItemPool.Release(scrollItemView);
        }


        private void InitPool(int initial = 10, int max = 20, bool collectionChecks = false)
        {
            _scrollItemPool = new ObjectPool<TScrollItemView>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max);
        }

        protected virtual TScrollItemView CreateSetup() => Instantiate(_scrolItemViewPrefab);
        protected virtual void GetSetup(TScrollItemView obj) => obj.gameObject.SetActive(true);
        protected virtual void ReleaseSetup(TScrollItemView obj) => obj.gameObject.SetActive(false);
        protected virtual void DestroySetup(TScrollItemView obj) => Destroy(obj);

        public TScrollItemView Get() => _scrollItemPool.Get();
        public void Release(TScrollItemView obj) => _scrollItemPool.Release(obj);
    }
}