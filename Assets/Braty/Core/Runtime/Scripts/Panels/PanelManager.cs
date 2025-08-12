using System;
using System.Collections.Generic;
using Braty.Core.Runtime.Scripts.MonoEcs;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public class PanelManager : MonoSystem
    {
        [SerializeField] private RectTransform _normalArea;
        [SerializeField] private RectTransform _safeArea;
        
        private readonly Dictionary<Type, PanelBase> _panels = new();

        public void ShowPanel<T>(bool isSafeArea) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't show");
                return;
            }

            var panel = GetPanel<T>();
            var parent = isSafeArea ? _safeArea : _normalArea;
            panel.transform.SetParent(parent, false);
            panel.gameObject.SetActive(true);
        }

        public void HidePanel<T>() where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Dynamic Panel {panelKey} is not loaded can't hide");
                return;
            }

            var panel = GetPanel<T>();
            panel.gameObject.SetActive(false);
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

        public void DestroyPanel<T>() where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't destroy");
                return;
            }
            Destroy(_panels[panelKey].gameObject);
        }

        internal void AddPanel<T>(T panel) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is already loaded");
                return;
            }

            _panels.Add(panelKey, panel);
        }

        internal void RemovePanel<T>(T panel) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.Remove(panelKey, out var panelObject))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't unload");
                return;
            }
        }
    }
}