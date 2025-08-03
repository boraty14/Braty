using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public class BSafeArea : MonoBehaviour
    {
        [SerializeField] private BCamera _bCamera;
        public bool ApplySafeAreaX;
        public bool ApplySafeAreaY;
        
        private Vector2Int _screenSize;
        private Rect _safeArea;
        
        private void Start()
        {
            ApplySafeArea();
        }

        private void Update()
        {
            if (Screen.safeArea != _safeArea
                || Screen.width != _screenSize.x
                || Screen.height != _screenSize.y)
            {
                ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            _screenSize = new Vector2Int(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;

            var cameraPosition = _bCamera.transform.position;
            var cameraVerticalSize = _bCamera.VerticalSize;

            float rawWidth = cameraVerticalSize * ((float)_screenSize.x / _screenSize.y) * 2f;
            float rawHeight = cameraVerticalSize * 2f;
            float safeWidth = rawWidth;
            float safeHeight = rawHeight;

            Vector3 offset = Vector3.zero;
            if (ApplySafeAreaX)
            {
                float leftRatio = _safeArea.x / _screenSize.x;
                float rightRatio = (_screenSize.x - (_safeArea.x + _safeArea.width)) / _screenSize.x;
                offset.x += -(rightRatio - leftRatio) * 0.5f * rawWidth;
                safeWidth *= 1 - (leftRatio + rightRatio);
            }

            if (ApplySafeAreaY)
            {
                float bottomRatio = _safeArea.y / _screenSize.y;
                float topRatio = (_screenSize.y - (_safeArea.y + _safeArea.height)) / _screenSize.y;
                offset.y += -(topRatio - bottomRatio) * 0.5f * rawHeight;
                safeHeight *= 1 - (bottomRatio + topRatio);
            }

            var safePosition = cameraPosition + offset;
            _bCamera.SetSafeScale(new Vector2(safeWidth / rawWidth, safeHeight / rawHeight),safePosition);
        }
    }
}