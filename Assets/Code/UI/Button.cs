using System;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public abstract class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;

        private Image Image => _image ?? GetComponent<Image>();

        public UnityEvent Clicked { get; } = new();

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.PauseMenuLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
            Image.color = new Color(0.8F, 0.9F, 0.9F, 1F);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainLeftClick.RemoveListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.PauseMenuLeftClick.RemoveListener(ActionType.Started, OnLeftMouseButton);
            Image.color = Color.white;
        }

        protected void OnDisable()
        {
            OnPointerExit(null);
        }

        private void OnLeftMouseButton(CallbackContext context)
        {
            Execute();
            Clicked.Invoke();
        }

        protected abstract void Execute();
    }
}