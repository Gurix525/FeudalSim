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

        public override void OnLeftMouseButton()
        {
            if (_leftClickable != null)
                _leftClickable.OnLeftMouseButton();
        }

        public override void OnRightMouseButton()
        {
            if (_rightclickable != null)
                _rightclickable.OnRightMouseButton();
        }

        public override void OnMouseOver(Component component)
        {
            if (Cursor.RaycastHit != null)
            {
                var converted = component as INoActionOutline;
                converted?.EnableOutline();
                _leftClickable = converted as ILeftClickHandler;
                _rightclickable = converted as IRightClickHandler;
            }
            else
            {
                (component as INoActionOutline)?.DisableOutline();
                _leftClickable = null;
                _rightclickable = null;
            }
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
            _leftClickable = null;
            _rightclickable = null;
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