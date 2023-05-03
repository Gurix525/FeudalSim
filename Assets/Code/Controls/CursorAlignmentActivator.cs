using Input;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public class CursorAlignmentActivator : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.MainAlt.AddListener(ActionType.Started, SwitchAlignement);
        }

        private void OnDisable()
        {
            PlayerController.MainAlt.RemoveListener(ActionType.Started, SwitchAlignement);
        }

        private void SwitchAlignement(CallbackContext context)
        {
            Cursor.AlignmentMultiplier
                *= Cursor.AlignmentMultiplier > 1F / 8F ? 1F / 2F : 8F;
            Cursor.Container.CollectionUpdated.Invoke();
        }
    }
}