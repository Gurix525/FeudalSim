using System;
using Controls;
using Misc;
using PlayerControls;
using UnityEngine;
using Cursor = Controls.Cursor;

namespace Items
{
    public abstract class ItemAction
    {
        #region Fields

        protected ILeftClickHandler _leftClickable;
        protected IRightClickHandler _rightclickable;

        protected Player Player => Player.Instance;
        protected PlayerMovement PlayerMovement => Player.Instance.PlayerMovement;

        #endregion Fields

        #region Properties

        public static NoAction NoAction { get; } = new();

        public Sprite Sprite => GetSprite();

        protected bool IsLeftClickPermitted => !CursorRaycaster.IsPointerOverGameObject
            && !PlayerMovement.IsPendingAttack
            && !PlayerMovement.IsStringingBow
            && PlayerMovement.IsGrounded;

        #endregion Properties

        #region Constructors

        public ItemAction()
        {
            Cursor.Container.CollectionUpdated.AddListener(OnCursorCollectionUpdated);
        }

        ~ItemAction()
        {
            Cursor.Container.CollectionUpdated.RemoveListener(OnCursorCollectionUpdated);
        }

        #endregion Constructors

        #region Public

        public virtual void OnLeftMouseButton()
        { }

        public virtual void OnLeftMouseButtonRelase()
        { }

        public virtual void OnRightMouseButton()
        { }

        public virtual void OnRightMouseButtonRelase()
        { }

        public virtual void Update()
        { }

        public virtual void OnMouseEnter(Component component)
        { }

        public virtual void OnMouseOver(Component component)
        { }

        public virtual void OnMouseExit(Component component)
        {
            (component as IOutline)?.DisableOutline();
        }

        #endregion Public

        #region Protected

        protected abstract Sprite GetSprite();

        #endregion Protected

        #region Private

        private void OnCursorCollectionUpdated()
        {
            OnMouseExit(null);
            OnLeftMouseButtonRelase();
            OnRightMouseButtonRelase();
        }

        #endregion Private
    }
}