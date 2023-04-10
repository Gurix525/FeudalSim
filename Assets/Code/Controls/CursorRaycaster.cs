using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        public static RaycastHit? Hit { get; private set; } = null;

        public static RaycastHit? CurrentHit => GetRaycastHit();

        private void Update()
        {
            GetRaycastHit();
        }

        private static RaycastHit? GetRaycastHit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Hit = null;
                return null;
            }
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            Hit = hit;
            return hit;
        }
    }
}