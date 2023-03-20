using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ItemActionExecutor : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerController.MainUse.AddListener(ActionType.Started, Execute);
    }

    private void OnDisable()
    {
        PlayerController.MainUse.ClearStartedEvent();
    }

    private void Execute(CallbackContext context)
    {
        LowerTerrain();
    }

    private void LowerTerrain()
    {
        if (Cursor.CellPosition != null)
        {
            Terrain.ModifyHeight((Vector2Int)Cursor.CellPosition, -1F);
            Debug.Log(Cursor.CellPosition);
        }
    }
}