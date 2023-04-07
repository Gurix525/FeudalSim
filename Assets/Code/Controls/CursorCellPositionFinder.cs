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
            if (Cursor.RaycastHit == null)
            {
                Cursor.CellPosition = null;
                return;
            }
            Cursor.CellPosition = new Vector2Int(
                Mathf.FloorToInt(Cursor.RaycastHit.Value.point.x),
                Mathf.FloorToInt(Cursor.RaycastHit.Value.point.z));
        }

        private void OnMouseExit()
        {
            Cursor.CellPosition = null;
        }

        #endregion Unity
    }
}