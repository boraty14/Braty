using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Cameras
{
    public class RootCamera : MonoBehaviour, IRootCamera
    {
        private readonly RootCameraBehaviour _rootCameraBehaviour;

        public RootCamera()
        {
            _rootCameraBehaviour = Instantiate(Resources.Load<RootCameraBehaviour>("RootCameraBehaviour"));
        }

        public Camera Camera => _rootCameraBehaviour.Camera;
    }
}