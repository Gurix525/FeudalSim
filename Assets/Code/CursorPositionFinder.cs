using UnityEngine;

public class CursorPositionFinder : MonoBehaviour
{
    private void OnMouseOver()
    {
        Ray ray = Camera.main
            .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
        Physics.Raycast(ray, out RaycastHit hit);
        Cursor.CellPosition = new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));

        //Debug.Log($"{hit.point}, {Cursor.CellPosition}");
    }

    private void OnMouseExit()
    {
        Cursor.CellPosition = null;
    }
}