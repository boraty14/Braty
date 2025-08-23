using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [DisallowMultipleComponent]
    public class BResolutionListener : MonoBehaviour
    {
        private Vector2Int _screenSize;
        private Rect _safeArea;
        
        public static Action OnResolutionChanged;

        private void Start()
        {
            OnResolutionChanged?.Invoke();
        }

        private void Update()
        {
            if (Screen.safeArea == _safeArea
                && Screen.width == _screenSize.x
                && Screen.height == _screenSize.y) return;
            
            _screenSize = new Vector2Int(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;
            OnResolutionChanged?.Invoke();
        }
    }
}