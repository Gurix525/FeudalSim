using Controls;
using UnityEngine;

namespace UI
{
    public class Header : Button, IMouseHandler
    {
        [SerializeField] private Window _window;

        private Vector2 _handleOffset;

        protected override void Execute()
        { }

        public new void OnLeftMouseButton(Vector2 position)
        {
            if (_window == null)
                return;
            _handleOffset = (Vector2)_window.transform.position - position;
        }

        public void OnMousePosition(Vector2 position)
        {
            if (_window == null)
                return;
            _window.transform.position = _handleOffset + position;
        }
    }
}