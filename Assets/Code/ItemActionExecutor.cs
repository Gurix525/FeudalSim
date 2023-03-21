using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ItemActionExecutor : MonoBehaviour
{
    private float _delta = -1F;
    private int _mode = 0;

    private void OnEnable()
    {
        PlayerController.MainUse.AddListener(ActionType.Started, Execute);
        PlayerController.MainChange.AddListener(ActionType.Started, ChangeMode);
    }

    private void OnDisable()
    {
        PlayerController.MainUse.ClearStartedEvent();
        PlayerController.MainChange.ClearStartedEvent();
    }

    private void Execute(CallbackContext context)
    {
        switch (_mode)
        {
            case 0:
            case 1:
                ModifyTerrainHeight();
                break;

            case 2:
                Pathen();
                break;

            default:
                Plow();
                break;
        }
    }

    private void ChangeMode(CallbackContext context)
    {
        _delta *= -1F;
        _mode++;
        if (_mode == 4)
            _mode = 1;
    }

    private void Plow()
    {
        if (Cursor.CellPosition != null)
            Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Color.green);
    }

    private void Pathen()
    {
        if (Cursor.CellPosition != null)
            Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Color.red);
    }

    private void ModifyTerrainHeight()
    {
        if (Cursor.CellPosition != null)
            Terrain.ModifyHeight((Vector2Int)Cursor.CellPosition, _delta);
    }
}