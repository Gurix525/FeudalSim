using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        private static bool _isPointerOverGameObject = false;

        public static float MaxCursorDistanceFromPlayer { get; } = 4F;

        public static RaycastHit? Hit { get; private set; } = null;

        public static RaycastHit? CurrentHit => GetRaycastHit();

        private void Update()
        {
            _isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
            GetRaycastHit();
        }

        private static RaycastHit? GetRaycastHit()
        {
            if (_isPointerOverGameObject)
            {
                Hit = null;
                return null;
            }
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            if (Vector3.Distance(hit.point, Player.Position) > MaxCursorDistanceFromPlayer
                && Vector3.Distance(hit.collider.transform.position, Player.Position) > MaxCursorDistanceFromPlayer)
            {
                Hit = null;
                return null;
            }
            Hit = hit;
            return hit;
        }
    }
}