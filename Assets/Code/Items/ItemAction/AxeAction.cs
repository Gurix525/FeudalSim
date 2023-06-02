using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Misc;

namespace Items
{
    public class AxeAction : ItemAction
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
                var converted = component as IAxeActionOutline;
                converted?.EnableOutline();
                _leftClickable = converted as ILeftClickHandler;
            }
            else
            {
                (component as IAxeActionOutline)?.DisableOutline();
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
            return Resources.Load<Sprite>("Sprites/Actions/AxeAction");
        }

        #endregion Protected
    }
}