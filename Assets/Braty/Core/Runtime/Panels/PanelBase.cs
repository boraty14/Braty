using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Braty.Core.Runtime.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public abstract class PanelBase : MonoBehaviour, IPanel
    {
        CanvasGroup IPanel.CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = GetComponent<CanvasGroup>();
                }

                return _canvasGroup;
            }
        }
        RectTransform IPanel.RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        
        bool IPanel.IsShown => _isShown;
        
        private bool _isShown;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        
        async UniTask IPanel.Show()
        {
            if (_isShown)
            {
                Debug.LogWarning($"Panel {gameObject.name} has already been shown.");
                return;
            }

            gameObject.SetActive(true);
            await OnOpening();
            OnOpened();
            _isShown = true;
        }

        async UniTask IPanel.Hide()
        {
            if (!_isShown)
            {
                Debug.LogWarning($"Panel {gameObject.name} has already been hidden.");
                return;
            }
            
            await OnClosing();
            OnClosed();
            gameObject.SetActive(false);
            _isShown = false;
        }

        protected virtual async UniTask OnOpening()
        {
        }

        protected virtual void OnOpened()
        {
        }

        protected virtual async UniTask OnClosing()
        {
        }

        protected virtual void OnClosed()
        {
        }
    }
}