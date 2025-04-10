using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Cameras
{
    public class RootCameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera _rootCamera;
        public Camera Camera => _rootCamera;
    }
}