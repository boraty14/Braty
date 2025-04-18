using System.Collections;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public abstract class PanelBase : MonoBehaviour
    {
        public CanvasGroup CanvasGroup { get; private set; }
        public RectTransform RectTransform { get; private set; }
        public bool IsShown { get; private set; }

        internal IEnumerator Show()
        {
            if (IsShown)
            {
                Debug.LogWarning($"Panel {gameObject.name} has already been shown.");
                yield break;
            }

            yield return ShowRoutine();
        }

        internal IEnumerator Hide()
        {
            if (!IsShown)
            {
                Debug.LogWarning($"Panel {gameObject.name} has already been hidden.");
                yield break;
            }

            yield return HideRoutine();
        }

        private IEnumerator ShowRoutine()
        {
            IsShown = true;
            gameObject.SetActive(true);
            yield return OnOpening();
            yield return OnOpened();
        }

        private IEnumerator HideRoutine()
        {
            IsShown = false;
            yield return OnClosing();
            yield return OnClosed();
            gameObject.SetActive(false);
        }

        protected virtual IEnumerator OnOpening()
        {
            yield break;
        }

        protected virtual IEnumerator OnOpened()
        {
            yield break;
        }

        protected virtual IEnumerator OnClosing()
        {
            yield break;
        }

        protected virtual IEnumerator OnClosed()
        {
            yield break;
        }
    }
}