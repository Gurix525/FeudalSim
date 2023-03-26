using Input;
using UnityEngine;

namespace Controls
{
    public class CursorPositionFinder : MonoBehaviour
    {
        private void OnMouseOver()
        {
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            Cursor.CellPosition = new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
        }

        private void OnMouseExit()
        {
            Cursor.CellPosition = null;
        }
    }
}