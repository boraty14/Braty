using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Panels
{
    public class PanelHolderBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _safeArea;
        [SerializeField] private Transform _normalArea;
        [SerializeField] private Canvas _parentCanvas;

        public Camera PanelCamera => _parentCanvas.worldCamera;

        public Transform SafeArea => _safeArea;
        public Transform NormalArea => _normalArea;
        public Canvas ParentCanvas => _parentCanvas;

        public void SetCamera(Camera panelCamera)
        {
            _parentCanvas.worldCamera = panelCamera;
        }
    }
}