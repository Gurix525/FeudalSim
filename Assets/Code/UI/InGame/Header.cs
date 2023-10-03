 
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class Header : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Window _window;
        private bool _isDragging = false;
        private Vector2 _initialMousePosition;

        // To be added
        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    PlayerController.MainLeftClick.AddListener(ActionType.Started, StartDraggingWindow);
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    PlayerController.MainLeftClick.RemoveListener(ActionType.Started, StartDraggingWindow);
        //}

        private void Update()
        {
            // To be added
            //if (_isDragging)
            //{
            //    Vector2 currentMousePosition = PlayerController.MainPoint.ReadValue<Vector2>();
            //    _window.CurrentOffset += currentMousePosition - _initialMousePosition;
            //    _initialMousePosition = currentMousePosition;
            //}
        }

        // To be added
        //private void OnDisable()
        //{
        //    OnPointerExit(null);
        //}

        //private void StartDraggingWindow(CallbackContext context)
        //{
        //    _isDragging = true;
        //    _initialMousePosition = PlayerController.MainPoint.ReadValue<Vector2>();
        //    PlayerController.MainLeftClick.AddListener(ActionType.Canceled, StopDraggingWindow);
        //}

        //private void StopDraggingWindow(CallbackContext obj)
        //{
        //    _isDragging = false;
        //    PlayerController.MainLeftClick.RemoveListener(ActionType.Canceled, StopDraggingWindow);
        //}
    }
}