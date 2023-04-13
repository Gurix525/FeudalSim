using UnityEngine;

namespace Items
{
    public abstract class ItemAction
    {
        public Sprite Sprite => GetSprite();

        public virtual void Execute()
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