using UnityEngine;
using UnityEngine.InputSystem;

namespace Braty.Core.Runtime.Scripts.BUI
{
    public class BCamera : MonoBehaviour
    {
        
        [SerializeField] private Camera _uiCamera;
        
        private RaycastHit2D[] _hitResults = new RaycastHit2D[50];
        // Cache the layer mask since it won't change
        private int _layerMask;

        private void Start()
        {
            // Get the layer mask for the "BUI" layer
            _layerMask = LayerMask.GetMask(BConstants.UILayerName);
            if (_layerMask == 0)
            {
                Debug.LogWarning("Layer 'BUI' not found. Please create it in the Layers dropdown.");
            }
        }

        private void Update()
        {
            // Get the mouse position from the new Input System
            // Note: Mouse.current can be null if a mouse is not connected or active
            if (Mouse.current == null)
            {
                return;
            }

            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

            // Convert the mouse screen position to a world position for an orthographic camera.
            // For an orthographic camera, you can set the z-value to the camera's near clip plane
            // to get a point on the plane closest to the camera.
            Vector3 mouseWorldPosition = _uiCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x,
                mouseScreenPosition.y, _uiCamera.nearClipPlane));

            // Since we are dealing with a 2D-like orthographic view, we can define the ray
            // from the mouse world position, pointing "out" from the camera.
            Ray ray = new Ray(mouseWorldPosition, _uiCamera.transform.forward);
            
            // Perform the non-allocating raycast
            int hitCount =
                Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hitResults, Mathf.Infinity, _layerMask);

            // Check if any objects were hit
            if (hitCount > 0)
            {
                // Iterate through the hits (up to hitCount)
                for (int i = 0; i < hitCount; i++)
                {
                    // Access the hit information from the m_Results array
                    RaycastHit2D hit = _hitResults[i];
                    Debug.Log("Hit object on BUI layer: " + hit.collider.gameObject.name);
                }
            }
        }
    }
}