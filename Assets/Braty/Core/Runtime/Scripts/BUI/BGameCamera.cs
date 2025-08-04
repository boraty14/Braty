using System;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(Camera))]
    public class BGameCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _targetRatio = new Vector2Int(16, 9);
        
        private Camera _gameCamera;
        private Vector2Int _screenSize;

        private void Awake()
        {
            _gameCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            SetRatio();
        }

        private void SetRatio()
        {
            Vector2Int newSize = new Vector2Int(Screen.width, Screen.height);
            if (newSize.x == _screenSize.x && newSize.y == _screenSize.y) return;

            _screenSize = newSize;
            float currentAspect = (float)_screenSize.x / (float)_screenSize.y;

            // Compare the current aspect ratio to the target aspect ratio
            float targetAspect = (float)_targetRatio.x / (float)_targetRatio.y;
            if (currentAspect > targetAspect)
            {
                // The screen is wider than the target (pillarbox effect)
                float width = targetAspect / currentAspect;
                float start = (1.0f - width) / 2.0f;
                _gameCamera.rect = new Rect(start, 0.0f, width, 1.0f);
            }
            else if (currentAspect < targetAspect)
            {
                // The screen is taller than the target (letterbox effect)
                float height = currentAspect / targetAspect;
                float start = (1.0f - height) / 2.0f;
                _gameCamera.rect = new Rect(0.0f, start, 1.0f, height);
            }
            else
            {
                // The screen is already the target aspect ratio,
                // so we set the camera rect to the full screen
                _gameCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            }
        }
    }
}