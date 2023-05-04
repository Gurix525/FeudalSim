using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Buildings;
using System.Threading.Tasks;
using Input;
using UnityEngine.InputSystem;
using System;
using static UnityEngine.InputSystem.InputAction;
using Misc;

namespace Items
{
    public class AxeAction : ItemAction
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
                (component as IAxeActionOutline)?.EnableOutline();
                _leftClickable = component as ILeftClickHandler;
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