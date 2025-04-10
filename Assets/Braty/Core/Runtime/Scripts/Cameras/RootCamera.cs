using Braty.Core.Runtime.Scripts.Panels;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Cameras
{
    public class RootCamera : MonoBehaviour, IRootCamera
    {
        [SerializeField] private Camera _rootCamera;
        public Camera Camera => _rootCamera;
    }
}