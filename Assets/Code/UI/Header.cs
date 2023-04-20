using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class Header : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Window _window;
        private bool _isDragging = false;
        private Vector2 _initialMousePosition;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainUse.AddListener(ActionType.Started, StartDraggingWindow);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, StartDraggingWindow);
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector2 currentMousePosition = PlayerController.MainPoint.ReadValue<Vector2>();
                _window.CurrentOffset += currentMousePosition - _initialMousePosition;
                _initialMousePosition = currentMousePosition;
            }
        }

        private void OnDisable()
        {
            OnPointerExit(null);
        }

        private void StartDraggingWindow(CallbackContext context)
        {
            _isDragging = true;
            _initialMousePosition = PlayerController.MainPoint.ReadValue<Vector2>();
            PlayerController.MainUse.AddListener(ActionType.Canceled, StopDraggingWindow);
        }

        private void StopDraggingWindow(CallbackContext obj)
        {
            _isDragging = false;
            PlayerController.MainUse.RemoveListener(ActionType.Canceled, StopDraggingWindow);
        }
    }
}