using Controls;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using Cursor = Controls.Cursor;

namespace Items
{
    public class NoAction : ItemAction
    {
        #region Public

        private IClickable _clickable;

        public override void Execute()
        {
            if (_clickable != null)
                _clickable.Click();
        }

        public override void OnMouseOver(Component component)
        {
            if (Cursor.RaycastHit != null)
            {
                (component as ItemHandler)?.EnableOutline();
                _clickable = component as IClickable;
            }
            else
            {
                (component as ItemHandler)?.DisableOutline();
                _clickable = null;
            }
        }

        public override void OnMouseExit(Component component)
        {
            (component as ItemHandler)?.DisableOutline();
            _clickable = null;
        }

        #endregion Public

        #region Protected

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/NoAction");
        }

        #endregion Protected
    }
}