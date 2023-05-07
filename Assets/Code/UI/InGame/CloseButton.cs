using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _windowToClose;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainLeftClick.AddListener(ActionType.Started, CloseWindow);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainLeftClick.RemoveListener(ActionType.Started, CloseWindow);
        }

        private void CloseWindow(CallbackContext context)
        {
            _windowToClose.SetActive(false);
            PlayerController.MainLeftClick.RemoveListener(ActionType.Started, CloseWindow);
        }
    }
}