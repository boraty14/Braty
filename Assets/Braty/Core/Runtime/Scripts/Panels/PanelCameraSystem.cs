using Braty.Core.Runtime.Scripts.MonoEcs;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    [RequireComponent(typeof(Camera))]
    public class PanelCameraSystem : MonoSystem
    {
        private Camera _camera;
        public Camera Camera => _camera;

        protected override void Awake()
        {
            base.Awake();
            _camera = GetComponent<Camera>();
        }
    }
}