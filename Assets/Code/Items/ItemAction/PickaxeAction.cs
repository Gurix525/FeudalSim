using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Misc;

namespace Items
{
    public class PickaxeAction : ItemAction
    {
        #region Fields

        private ILeftClickHandler _leftClickable;

        #endregion Fields

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
                (component as IPickaxeActionOutline)?.EnableOutline();
                _leftClickable = component as ILeftClickHandler;
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