using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Buildings;
using System.Threading.Tasks;
using Input;
using UnityEngine.InputSystem;
using System;
using static UnityEngine.InputSystem.InputAction;

namespace Items
{
    public class DestroyAction : ItemAction
    {
        #region Fields

        private Building _buildingToDestroy = null;
        private bool _isWaitingForAnotherBuilding = false;

        #endregion Fields

        #region Public

        public override void Execute()
        {
            if (Cursor.CurrentRaycastHit == null)
                return;
            _buildingToDestroy = Cursor.RaycastHit.Value.transform.GetComponent<Building>();
            if (_buildingToDestroy == null)
                return;
            _ = ActionTimer.Start(FinishExecution, 1F);
        }

        public override void OnMouseEnter(Component component)
        {
            (component as Building)?.ChangeColor(new(1F, 0.75F, 0.75F));
        }

        public override void OnMouseOver(Component component)
        {
            _buildingToDestroy = component as Building;
            if (_buildingToDestroy != null && _isWaitingForAnotherBuilding)
            {
                DisableWaiting(new());
                Execute();
            }
        }

        public override void OnMouseExit(Component component)
        {
            (component as Building)?.ResetColor();
        }

        #endregion Public

        #region Private

        private void FinishExecution()
        {
            if (Cursor.RaycastHit == null)
                return;
            Building buildingToDestroy = Cursor.RaycastHit.Value.transform.GetComponent<Building>();
            if (buildingToDestroy == null)
                buildingToDestroy = _buildingToDestroy;
            GameObject.Destroy(buildingToDestroy.gameObject);
            _isWaitingForAnotherBuilding = true;
            PlayerController.MainUse.AddListener(ActionType.Canceled, DisableWaiting);
        }

        private void DisableWaiting(CallbackContext context)
        {
            _isWaitingForAnotherBuilding = false;
            PlayerController.MainUse.RemoveListener(ActionType.Canceled, DisableWaiting);
        }

        #endregion Private
    }
}