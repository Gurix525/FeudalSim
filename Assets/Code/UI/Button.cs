using System;
using Controls;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public abstract class Button : MonoBehaviour, ILeftMouseButtonHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler Clicked;

        private Image _image;
        private Color _originalColor;

        private Image Image => _image ?? GetComponent<Image>();


        private void Awake()
        {
            _originalColor = Image.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Image.color = new Color(0.8F, 0.9F, 0.9F, _originalColor.a);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Image.color = _originalColor;
        }

        public void OnLeftMouseButton()
        {
            Execute();
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDisable()
        {
            OnPointerExit(null);
        }

        protected abstract void Execute();
    }
}