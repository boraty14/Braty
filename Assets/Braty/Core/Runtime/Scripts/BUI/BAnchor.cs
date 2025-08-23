using Braty.Core.Runtime.Scripts.MonoEcs;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public class BAnchor : MonoBehaviour
    {
        [SerializeField] private Vector2 _anchor;

        private void Start()
        {
            SetAnchor(_anchor);
        }

        private void OnEnable()
        {
            BResolutionListener.OnResolutionChanged += OnResolutionChanged;
        }

        private void OnDisable()
        {
            BResolutionListener.OnResolutionChanged -= OnResolutionChanged;
        }
        
        public void SetAnchor(Vector2 anchor)
        {
            _anchor = anchor;
            float cameraVerticalSize = MonoManager.GetSystem<BCameraSystem>().VerticalSize;
            float width = cameraVerticalSize * ((float)Screen.width / Screen.height);
            float height = cameraVerticalSize;
            transform.localPosition = new Vector3(_anchor.x * width, _anchor.y * height, transform.localPosition.z);
        }

        private void OnResolutionChanged()
        {
            SetAnchor(_anchor);
        }
    }
}