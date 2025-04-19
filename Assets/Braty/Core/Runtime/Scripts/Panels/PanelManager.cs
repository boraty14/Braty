using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _safeArea;
        [SerializeField] private RectTransform _normalArea;

        private readonly Dictionary<Type, GameObject> _panels = new();
        public static PanelManager I { get; private set; }

        private void Awake()
        {
            I = this;
        }

        public event Action<PanelBase> OnPanelOpening;
        public event Action<PanelBase> OnPanelOpened;
        public event Action<PanelBase> OnPanelClosing;
        public event Action<PanelBase> OnPanelClosed;

        public void ShowPanel<T>(bool isSafeArea) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't show");
                return;
            }

            var newParent = isSafeArea ? _safeArea : _normalArea;

            StartCoroutine(ShowRoutine());
            return;

            IEnumerator ShowRoutine()
            {
                var panel = GetPanel<T>();
                if (panel.IsShown)
                {
                    Debug.LogError($"Panel {panelKey} is already shown");
                    yield break;
                }

                panel.RectTransform.SetParent(newParent, false);
                OnPanelOpening?.Invoke(panel);
                yield return panel.Show();
                OnPanelOpened?.Invoke(panel);
            }
        }

        public void HidePanel<T>(bool unloadPanel) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Dynamic Panel {panelKey} is not loaded can't hide");
                return;
            }

            StartCoroutine(HideRoutine());
            return;

            IEnumerator HideRoutine()
            {
                var panel = GetPanel<T>();
                if (!panel.IsShown)
                {
                    Debug.LogError($"Panel {panelKey} is already not shown");
                    yield break;
                }

                OnPanelClosing?.Invoke(panel);
                yield return panel.Hide();
                OnPanelClosed?.Invoke(panel);
                if (unloadPanel)
                {
                    UnloadPanel<T>();
                }
            }
        }

        public T GetPanel<T>()
        {
            return _panels[typeof(T)].GetComponent<T>();
        }

        public IEnumerator LoadPanel<T>(Transform newParent) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is already loaded");
                yield break;
            }

            var panelHandle = Addressables.InstantiateAsync(typeof(T).Name, newParent);
            yield return panelHandle;
            var panelObject = panelHandle.Result;
            panelObject.SetActive(false);
            _panels.Add(panelKey, panelObject);
        }

        private void UnloadPanel<T>() where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.Remove(panelKey, out var panelObject))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't unload");
                return;
            }

            Addressables.ReleaseInstance(panelObject);
        }
    }
}