using System.Collections.Generic;
using SurvivalShooter.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace SurvivalShooter.AR
{
    /// <summary>Tap-to-place game world once.</summary>
    public class ARPlacementController : MonoBehaviour
    {
        [SerializeField] ARRaycastManager raycastManager;
        [SerializeField] ARPlaneManager planeManager;
        [SerializeField] ARAnchorManager anchorManager;
        [SerializeField] GameObject gameWorldPrefab;

        static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

        public Transform PlacedWorld { get; private set; }
        public bool HasPlaced => PlacedWorld != null;

        void Awake()
        {
            planeManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
        }

        void Update()
        {
            if (HasPlaced) return;
            if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameStateId.Placement) return;
            if (!TryGetTap(out Vector2 screenPos)) return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            if (raycastManager.Raycast(screenPos, Hits, TrackableType.PlaneWithinPolygon))
                _ = PlaceAsync(Hits[0].pose);
        }

        async Awaitable PlaceAsync(Pose pose)
        {
            if (HasPlaced) return;

            // Claim before anchoring.
            var world = Instantiate(gameWorldPrefab, pose.position, Quaternion.Euler(0f, pose.rotation.eulerAngles.y, 0f));
            PlacedWorld = world.transform;

            if (anchorManager != null)
            {
                var result = await anchorManager.TryAddAnchorAsync(pose);
                if (result.status.IsSuccess())
                    PlacedWorld.SetParent(result.value.transform, true);
            }

            // Stop plane detection.
            planeManager.SetTrackablesActive(false);
            planeManager.enabled = false;

            GameEvents.RaiseGameWorldPlaced();
        }

        static bool TryGetTap(out Vector2 position)
        {
            var touch = Touchscreen.current;
            if (touch != null && touch.primaryTouch.press.wasPressedThisFrame)
            {
                position = touch.primaryTouch.position.ReadValue();
                return true;
            }

            var mouse = Mouse.current; // Editor testing
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                position = mouse.position.ReadValue();
                return true;
            }

            position = default;
            return false;
        }
    }
}
