using System;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public abstract class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainLeftClick.RemoveListener(ActionType.Started, OnLeftMouseButton);
        }

        protected void OnDisable()
        {
            OnPointerExit(null);
        }

        private void OnLeftMouseButton(CallbackContext context)
        {
            Execute();
        }

        protected abstract void Execute();
    }
}