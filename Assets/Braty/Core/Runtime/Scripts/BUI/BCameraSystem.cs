using System.Collections.Generic;
using Braty.Core.Runtime.Scripts.MonoEcs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Braty.Core.Runtime.Scripts.BUI
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(BResolutionListener))]
    [DisallowMultipleComponent]
    public class BCameraSystem : MonoSystem
    {
        [SerializeField] private LayerMask _layerMask;
        private Camera _uiCamera;
        private readonly RaycastHit2D[] _hitResults = new RaycastHit2D[50];
        private readonly List<BInteractable> _currentInteractables = new();

        private readonly List<BInteractable> _currentHovers = new();
        private readonly Stack<BInteractable> _currentHoversRemoveStack = new();

        public float VerticalSize => _uiCamera.orthographicSize;

        private Vector2Int _screenSize;

        protected override void Awake()
        {
            base.Awake();
            _uiCamera = GetComponent<Camera>();
        }

        private void Update()
        {
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

            // reset current interactables
            _currentInteractables.Clear();

            // Check if any objects were hit, order them by priority
            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    RaycastHit2D hit = _hitResults[i];
                    if (hit.collider.TryGetComponent<BInteractable>(out var interactable))
                    {
                        _currentInteractables.Add(interactable);
                    }
                }

                // sort current interactables by prio
                _currentInteractables.Sort((item1, item2) => item2.Priority.CompareTo(item1.Priority));

                // int find remove range index and remove them
                int maxPriority = 0;
                for (int i = 0; i < _currentInteractables.Count; i++)
                {
                    var interactable = _currentInteractables[i];
                    if (i == 0)
                    {
                        maxPriority = interactable.Priority;
                    }

                    if (interactable.Priority < maxPriority)
                    {
                        _currentInteractables.RemoveRange(i, _currentInteractables.Count - i);
                        break;
                    }
                }
            }

            foreach (var interactable in _currentInteractables)
            {
                // Mouse Enter
                if (!_currentHovers.Contains(interactable))
                {
                    _currentHovers.Add(interactable);
                    interactable.MouseEnterEvent(ray.origin);
                }

                // Mouse Over
                interactable.MouseOverEvent(ray.origin);
                
                // Mouse Down
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    interactable.MouseDownEvent(ray.origin);
                }
                // Mouse Drag
                else if (Mouse.current.leftButton.isPressed)
                {
                    interactable.MouseDragEvent(ray.origin);
                }
                // Mouse Up
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    interactable.MouseUpEvent(ray.origin);
                }
                
                // Right Click
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    interactable.RightClickEvent(ray.origin);
                }
            }

            // Mouse Exit
            _currentHoversRemoveStack.Clear();
            foreach (var hover in _currentHovers)
            {
                if (!_currentInteractables.Contains(hover))
                {
                    hover.MouseExitEvent(ray.origin);
                    _currentHoversRemoveStack.Push(hover);
                }
            }

            // remove exited items from hover/mouseover
            while (_currentHoversRemoveStack.TryPop(out var hover))
            {
                _currentHovers.Remove(hover);
            }
        }
    }
}