using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorCellPositionFinder : MonoBehaviour
    {
        #region Unity

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.CellPosition = null;
                return;
            }
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            Cursor.CellPosition = new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
        }

        private void OnMouseExit()
        {
            Cursor.CellPosition = null;
        }

        #endregion Unity
    }
}