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
        //Debug.Log(Cursor.Cell);
        //if (Cursor.IsAboveTerrain)
        //    Terrain.LowerTerrain(Cursor.Cell);
    }
}