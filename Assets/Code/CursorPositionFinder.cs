using UnityEngine;

public class CursorPositionFinder : MonoBehaviour
{
    private void OnMouseOver()
    {
        Ray ray = Camera.main
            .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
        Physics.Raycast(ray, out RaycastHit hit);
        Cursor.TerrainCell = Terrain.GetCell(hit.point);
        //Debug.Log(Cursor.TerrainCell);
    }

    private void OnMouseExit()
    {
        Cursor.TerrainCell = null;
    }
}