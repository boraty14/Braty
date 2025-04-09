using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public class PanelManager : MonoBehaviour, IPanelManager
    {
        [SerializeField] private Transform _safeArea;
        [SerializeField] private Transform _normalArea;
        
        private readonly Dictionary<Type, GameObject> _panels = new();

        private event Action<IPanel> OnPanelOpening;
        event Action<IPanel> IPanelManager.OnPanelOpening
        {
            add => this.OnPanelOpening += value;
            remove => this.OnPanelOpening -= value;
        }
        private event Action<IPanel> OnPanelOpened;
        event Action<IPanel> IPanelManager.OnPanelOpened
        {
            add => this.OnPanelOpened += value;
            remove => this.OnPanelOpened -= value;
        }
        private event Action<IPanel> OnPanelClosing;
        event Action<IPanel> IPanelManager.OnPanelClosing
        {
            add => this.OnPanelClosing += value;
            remove => this.OnPanelClosing -= value;
        }
        private event Action<IPanel> OnPanelClosed;
        event Action<IPanel> IPanelManager.OnPanelClosed
        {
            add => this.OnPanelClosed += value;
            remove => this.OnPanelClosed -= value;
        }

        async UniTask IPanelManager.ShowPanel<T>(bool isSafeArea)
        {
            var panelKey = typeof(T);
            var newParent = isSafeArea ? _safeArea : _normalArea;
            if (!_panels.ContainsKey(panelKey))
            {
                await LoadPanel<T>(newParent);
            }

            if (!_panels.TryGetValue(panelKey, out var panelObject))
            {
                Debug.LogError($"Panel {panelKey} is not loaded can't show");
                return;
            }

            var panel = panelObject.GetComponent<T>();
            panel.RectTransform.SetParent(newParent,false);
            OnPanelOpening?.Invoke(panel);
            await panel.Show();
            OnPanelOpened?.Invoke(panel);
        }

        async UniTask IPanelManager.HidePanel<T>(bool unloadPanel)
        {
            var dynamicPanelKey = typeof(T);
            if (!_panels.TryGetValue(dynamicPanelKey, out var dynamicPanelObject))
            {
                Debug.LogError($"Dynamic Panel {dynamicPanelKey} is not loaded can't hide");
                return;
            }

            var dynamicPanel = dynamicPanelObject.GetComponent<T>();
            OnPanelClosing?.Invoke(dynamicPanel);
            await dynamicPanel.Hide();
            OnPanelClosed?.Invoke(dynamicPanel);
            if (unloadPanel)
            {
                UnloadPanel<T>();
            }
        }
        
        T IPanelManager.GetPanel<T>()
        {
            return _panels[typeof(T)].GetComponent<T>();
        }
        
        private void Awake()
        {
            var panels = GetComponentsInChildren<PanelBase>(true);
            foreach (var panel in panels)
            {
                var basePanelKey = panel.GetType();
                var panelKey = basePanelKey.GetInterfaces()
                    .FirstOrDefault(t => t != typeof(IPanel) && typeof(IPanel).IsAssignableFrom(t));
                _panels.TryAdd(panelKey, panel.gameObject);
            }
        }
        
        private async UniTask LoadPanel<T>(Transform newParent) where T : IPanel
        {
            var panelKey = typeof(T);
            if (_panels.ContainsKey(panelKey))
            {
                Debug.LogError($"Panel {panelKey} is already loaded");
            }

            var panelObject = await Addressables.InstantiateAsync(typeof(T).Name, newParent);
            panelObject.SetActive(false);
            _panels.Add(panelKey, panelObject);
        }

        private void UnloadPanel<T>() where T : IPanel
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