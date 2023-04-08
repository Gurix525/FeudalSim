using UnityEngine;

namespace Items
{
    public abstract class ItemAction
    {
        public virtual void Execute()
        { }

        public virtual void Update()
        { }

        public virtual void OnMouseEnter(Component component)
        { }

        public virtual void OnMouseExit(Component component)
        { }
    }
}