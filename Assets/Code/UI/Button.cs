using System;
using Controls;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Button : MonoBehaviour, IMouseHandler
    {
        public event EventHandler Clicked;

        private Image _image;
        private Color _originalColor;

        private Image Image => _image ?? GetComponent<Image>();

        private void Awake()
        {
            _originalColor = Image.color;
        }

        public void OnHoverStart()
        {
            Image.color = new Color(0.8F, 0.9F, 0.9F, _originalColor.a);
        }

        public void OnHoverEnd()
        {
            Image.color = _originalColor;
        }

        public void OnLeftMouseButton(Vector2 position)
        {
            Execute();
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDisable()
        {
            OnHoverEnd();
        }

        protected virtual void Execute()
        { }
    }
}