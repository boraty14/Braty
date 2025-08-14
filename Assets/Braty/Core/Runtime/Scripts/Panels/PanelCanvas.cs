using Braty.Core.Runtime.Scripts.MonoEcs;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    [RequireComponent(typeof(Canvas))]
    public class PanelCanvas : MonoBehaviour
    {
        private Canvas _canvas;
        
        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = MonoManager.GetSystem<PanelCameraSystem>().Camera;
        }
    }
}