using System;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public static class PanelManager
    {
        private static readonly Dictionary<Type, PanelBase> _panels = new();

        public static void Init() => _panels.Clear();

        public static void ShowPanel<T>(bool isSafeArea) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't show");
                return;
            }

            var panel = GetPanel<T>();
            panel.gameObject.SetActive(true);
        }

        public static void HidePanel<T>() where T : PanelBase
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

        public static T GetPanel<T>()
        {
            return _panels[typeof(T)].GetComponent<T>();
        }

        public static bool IsPanelShown<T>()
        {
            var panelKey = typeof(T);
            return IsPanelLoaded<T>() && _panels[panelKey].gameObject.activeInHierarchy;
        }

        public static bool IsPanelLoaded<T>()
        {
            var panelKey = typeof(T);
            return _panels.ContainsKey(panelKey);
        }

        internal static void AddPanel<T>(T panel) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.TryAdd(panelKey, panel))
            {
                Debug.LogError($"Panel {panelKey} is already loaded");
                return;
            }

            HidePanel<T>();
        }

        internal static void RemovePanel<T>(T panel) where T : PanelBase
        {
            var panelKey = typeof(T);
            if (!_panels.Remove(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't unload");
            }
        }
    }
}