using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public class Builder : MonoBehaviour
    {
        #region Fields

        private BuildingMode _buildingMode = BuildingMode.BigWall;

        #endregion Fields

        #region Unity

        private void OnEnable()
        {
            PlayerController.MainQuickMenu.AddListener(ActionType.Started, ChangeMode);
        }

        private void Update()
        {
            if (Cursor.Item == null)
            {
                CursorMeshHighlight.TrySetMesh(null);
                return;
            }
            if (Cursor.Item.IsEligibleForBuilding)
            {
                CursorMeshHighlight.TrySetMesh(_buildingMode switch
                {
                    BuildingMode.Floor => Cursor.Item.BuildingMeshes[0],
                    BuildingMode.BigFloor => Cursor.Item.BuildingMeshes[1],
                    BuildingMode.ShortWall => Cursor.Item.BuildingMeshes[2],
                    BuildingMode.Wall => Cursor.Item.BuildingMeshes[3],
                    _ => Cursor.Item.BuildingMeshes[4]
                });
            }
        }

        private void OnDisable()
        {
            PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, ChangeMode);
        }

        #endregion Unity

        #region Private

        private void ChangeMode(CallbackContext context)
        {
            _buildingMode = (int)(_buildingMode + 1) > 4 ? 0 : _buildingMode + 1;
        }

        #endregion Private
    }
}