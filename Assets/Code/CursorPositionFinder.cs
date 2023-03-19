using UnityEngine;

public class CursorPositionFinder : MonoBehaviour
{
    private void OnMouseOver()
    {
        Ray ray = Camera.main
            .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
        Physics.Raycast(ray, out RaycastHit hit);
        Cursor.Cell = Terrain.GetCell(hit.point);
        Debug.Log($"{hit.point}, {Cursor.Cell}");
    }

    private void OnMouseExit()
    {
        Cursor.Cell = null;
    }
}