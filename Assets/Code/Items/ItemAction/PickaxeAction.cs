using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Misc;
using Nature;

namespace Items
{
    public class PickaxeAction : ItemAction
    {
        #region Public

        public override void OnLeftMouseButton()
        {
            if (_leftClickable != null)
                _leftClickable.OnLeftMouseButton();
        }

        public override void OnMouseOver(Component component)
        {
            if (Cursor.RaycastHit != null)
            {
                var converted = component as IPickaxeActionOutline;
                converted?.EnableOutline();
                _leftClickable = converted as ILeftClickHandler;
            }
            else
            {
                (component as IPickaxeActionOutline)?.DisableOutline();
                _leftClickable = null;
            }
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
            _leftClickable = null;
        }

        #endregion Public

        #region Protected

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/PickaxeAction");
        }

        #endregion Protected
    }
}