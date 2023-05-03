using Input;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public class CursorControlActivator : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.MainControl.AddListener(ActionType.Started, EnableControlMode);
        }

        private void OnDisable()
        {
            PlayerController.MainControl.RemoveListener(ActionType.Started, EnableControlMode);
        }

        private void EnableControlMode(CallbackContext context)
        {
            Cursor.IsNoActionActive = true;
            Cursor.Container.CollectionUpdated.Invoke();
            PlayerController.MainControl.RemoveListener(ActionType.Started, EnableControlMode);
            PlayerController.MainControl.AddListener(ActionType.Canceled, DisableControlMode);
        }

        private void DisableControlMode(CallbackContext context)
        {
            Cursor.IsNoActionActive = false;
            Cursor.Container.CollectionUpdated.Invoke();
            PlayerController.MainControl.RemoveListener(ActionType.Canceled, DisableControlMode);
            PlayerController.MainControl.AddListener(ActionType.Started, EnableControlMode);
        }
    }
}