using Input;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace Items
{
    public class CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _windowToClose;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainUse.AddListener(ActionType.Started, CloseWindow);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, CloseWindow);
        }

        private void CloseWindow(CallbackContext context)
        {
            _windowToClose.SetActive(false);
            PlayerController.MainUse.RemoveListener(ActionType.Started, CloseWindow);
        }
    }
}