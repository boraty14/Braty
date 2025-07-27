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

        public void ShowPanel<T>(bool isSafeArea) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't show");
                return;
            }

            var newParent = isSafeArea ? _safeArea : _normalArea;
            var panel = GetPanel<T>();

            panel.GetComponent<RectTransform>().SetParent(newParent, false);
            panel.gameObject.SetActive(true);
        }

        public void HidePanel<T>(bool unloadPanel) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Dynamic Panel {panelKey} is not loaded can't hide");
                return;
            }

            var panel = GetPanel<T>();
            panel.gameObject.SetActive(false);

            if (unloadPanel)
            {
                UnloadPanel<T>();
            }
        }

        public T GetPanel<T>()
        {
            return _panels[typeof(T)].GetComponent<T>();
        }

        public bool IsPanelShown<T>()
        {
            var panelKey = typeof(T);
            return IsPanelLoaded<T>() && _panels[panelKey].gameObject.activeInHierarchy;
        }

        public bool IsPanelLoaded<T>()
        {
            var panelKey = typeof(T);
            return _panels.ContainsKey(panelKey);
        }

        public void LoadPanel<T>(Transform newParent, Action onComplete = null) where T : PanelBase
        {
            StartCoroutine(LoadPanelRoutine<T>(newParent, onComplete));
        }

        private IEnumerator LoadPanelRoutine<T>(Transform newParent, Action onComplete = null) where T : PanelBase
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
            onComplete?.Invoke();
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