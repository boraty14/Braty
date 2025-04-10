using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Cameras
{
    public class RootCamera : IRootCamera
    {
        private readonly RootCameraBehaviour _rootCameraBehaviour;

        public RootCamera()
        {
            _rootCameraBehaviour = Object.Instantiate(Resources.Load<RootCameraBehaviour>("RootCameraBehaviour"));
        }

        public Camera Camera => _rootCameraBehaviour.Camera;
    }
}