using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        private static bool _isPointerOverGameObject = false;

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
            Hit = hit;
            return hit;
        }
    }
}