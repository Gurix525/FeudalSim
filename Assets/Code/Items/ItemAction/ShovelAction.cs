using Controls;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine;
using Cursor = Controls.Cursor;
using System;
using Input;

namespace Items
{
    public class ShovelAction : ItemAction
    {
        private int _delta = -1;
        private ShovelMode _shovelMode = ShovelMode.Digging;
        private bool _isWaitingForAnotherCell = false;

        public ShovelAction()
        {
            Cursor.Container.CollectionUpdated.AddListener(OnCursorCollectionUpdated);
        }

        public override void Execute()
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
                Execute();
            }
        }

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
            _isWaitingForAnotherCell = true;
            PlayerController.MainUse.AddListener(ActionType.Canceled, DisableWaiting);
        }

        private void OnCursorCollectionUpdated()
        {
            if (Cursor.Item == null)
            {
                PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, ChangeMode);
                return;
            }
            if (Cursor.Item.Action != this)
            {
                PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, ChangeMode);
                return;
            }
            PlayerController.MainQuickMenu.AddListener(ActionType.Started, ChangeMode);
        }

        private void ChangeMode(CallbackContext context)
        {
            _delta *= -1;
            _shovelMode = (int)(_shovelMode + 1) > 3 ? 0 : _shovelMode + 1;
            //_modeText.text = _shovelMode switch
            //{
            //    ShovelMode.Digging => $"Tryb: kopanie",
            //    ShovelMode.Rising => $"Tryb: wznoszenie",
            //    ShovelMode.Pathing => $"Tryb: ścieżkowanie",
            //    _ => $"Tryb: oranie"
            //};
        }

        private void Plow()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Color.green);
        }

        private void Pathen()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Color.red);
        }

        private void ModifyTerrainHeight()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ModifyHeight((Vector2Int)Cursor.CellPosition, _delta);
        }

        private void DisableWaiting(CallbackContext context)
        {
            _isWaitingForAnotherCell = false;
            PlayerController.MainUse.RemoveListener(ActionType.Canceled, DisableWaiting);
        }
    }
}