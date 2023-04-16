using UnityEngine;

namespace Items
{
    public abstract class ItemAction
    {
        public static NoAction NoAction { get; } = new();

        public Sprite Sprite => GetSprite();

        public virtual void OnLeftMouseButton()
        { }

        public virtual void OnRightMouseButton()
        { }

        public virtual void Update()
        { }

        public virtual void OnMouseEnter(Component component)
        { }

        public virtual void OnMouseOver(Component component)
        { }

        public virtual void OnMouseExit(Component component)
        { }

        protected abstract Sprite GetSprite();
    }
}