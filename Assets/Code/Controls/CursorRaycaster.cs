using System;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerControls;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        public static bool IsPointerOverGameObject { get; private set; }

        public static float MaxCursorDistanceFromPlayer { get; } = 4F;

        public static RaycastHit? ClearHit { get; private set; } = null;

        public static RaycastHit? Hit { get; private set; } = null;

        public static RaycastHit? CurrentHit => GetRaycastHit();

        private void Update()
        {
            IsPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
            GetRaycastHit();
        }

        private static RaycastHit? GetRaycastHit()
        {
            try
            {
                if (IsPointerOverGameObject)
                {
                    ClearHit = null;
                    Hit = null;
                    return null;
                }
                Ray ray = Camera.main
                    .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
                Physics.Raycast(ray, out RaycastHit hit);
                if (Vector3.Distance(hit.point, Player.Position) > MaxCursorDistanceFromPlayer
                    && Vector3.Distance(hit.collider.transform.position, Player.Position) > MaxCursorDistanceFromPlayer)
                {
                    ClearHit = hit;
                    Hit = null;
                    return null;
                }
                ClearHit = hit;
                Hit = hit;
                return hit;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}