
using UnityEngine;

namespace Controls
{
    public interface IMouseHandler
    {
        public virtual void OnLeftMouseButton(Vector2 position) { }

        public virtual void OnLeftMouseButtonRelase() { }

        public virtual void OnRightMouseButton() { }

        public virtual void OnRightMouseButtonRelase() { }

        public virtual void OnHoverStart() { }

        public virtual void OnHoverEnd() { }

        public virtual void OnMouseDelta(Vector2 delta) { }

        public virtual void OnMousePosition(Vector2 position) { }
    }
}