using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorExactPositionFinder : MonoBehaviour
    {
        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.ExactPosition = null;
                return;
            }
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            Cursor.ExactPosition = hit.point;
        }
    }
}