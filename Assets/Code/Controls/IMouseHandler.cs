namespace Controls
{
    public interface IMouseHandler
    {
        public virtual void OnLeftMouseButton() { }

        public virtual void OnLeftMouseButtonRelase() { }

        public virtual void OnRightMouseButton() { }

        public virtual void OnRightMouseButtonRelase() { }

        public virtual void OnHoverStart() { }

        public virtual void OnHoverEnd() { }
    }
}