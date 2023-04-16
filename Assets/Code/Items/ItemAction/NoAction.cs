using Controls;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using Cursor = Controls.Cursor;

namespace Items
{
    public class NoAction : ItemAction
    {
        #region Fields

        private ILeftClickHandler _leftClickable;
        private IRightClickHandler _rightclickable;

        #endregion Fields

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
                (component as ItemHandler)?.EnableOutline();
                _leftClickable = component as ILeftClickHandler;
                _rightclickable = component as IRightClickHandler;
            }
            else
            {
                (component as ItemHandler)?.DisableOutline();
                _leftClickable = null;
            }
        }

        public override void OnMouseExit(Component component)
        {
            (component as ItemHandler)?.DisableOutline();
            _leftClickable = null;
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