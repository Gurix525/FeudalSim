using Controls;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine;
using Cursor = Controls.Cursor;
using System;
using Input;
using World;

namespace Items
{
    public class ShovelAction : ItemAction
    {
        #region Fields

        private int _delta = -1;
        private ShovelMode _shovelMode = ShovelMode.Digging;
        private bool _isWaitingForAnotherCell = false;

        #endregion Fields

        #region Public

        public void SetShovelMode(int modeNumber)
        {
            _shovelMode = (ShovelMode)modeNumber;
            _delta = modeNumber == 0 ? -1 : 1;
        }

        public override void OnLeftMouseButton()
        {
            if (Cursor.CellPosition != null)
            {
                _isWaitingForAnotherCell = false;
                DisableWaiting(new());
                _ = ActionTimer.Start(FinishExecution, 1F);
            }
        }

        public override void Update()
        {
            if (_isWaitingForAnotherCell)
            {
                OnLeftMouseButton();
            }
        }

        #endregion Public

        #region Private

        private void FinishExecution()
        {
            switch (_shovelMode)
            {
                case ShovelMode.Digging:
                case ShovelMode.Rising:
                    ModifyTerrainHeight();
                    break;

                case ShovelMode.Pathing:
                    Pathen();
                    break;

                default:
                    Plow();
                    break;
            }
            TerrainRenderer.MarkNavMeshToReload();
            _isWaitingForAnotherCell = true;
            PlayerController.MainLeftClick.AddListener(ActionType.Canceled, DisableWaiting);
        }

        private void Plow()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Cell.PathVerticeColor);
        }

        private void Pathen()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Cell.PathVerticeColor);
        }

        private void ModifyTerrainHeight()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ModifyHeight((Vector2Int)Cursor.CellPosition, _delta);
        }

        private void DisableWaiting(CallbackContext context)
        {
            _isWaitingForAnotherCell = false;
            PlayerController.MainLeftClick.RemoveListener(ActionType.Canceled, DisableWaiting);
        }

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/" + _shovelMode switch
            {
                ShovelMode.Digging => "ShovelActionDigging",
                ShovelMode.Rising => "ShovelActionRising",
                ShovelMode.Pathing => "ShovelActionPathing",
                ShovelMode.Plowing => "ShovelActionPlowing",
                _ => "Placeholder",
            });
        }

        #endregion Private
    }
}