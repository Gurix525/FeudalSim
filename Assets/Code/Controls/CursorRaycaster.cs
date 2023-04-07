using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        private static RaycastHit? _hit = null;

        public static RaycastHit? Hit => _hit;

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _hit = null;
                return;
            }
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            _hit = hit;
        }
    }
}