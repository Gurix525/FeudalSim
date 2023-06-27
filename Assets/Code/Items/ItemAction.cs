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

        protected Player _player;
        protected PlayerMovement _playerMovement;

        #endregion Fields

        #region Properties

        public static NoAction NoAction { get; } = new();

        public Sprite Sprite => GetSprite();

        protected bool IsLeftClickPermitted => !CursorRaycaster.IsPointerOverGameObject
            && !_playerMovement.IsPendingAttack
            && !_playerMovement.IsStringingBow
            && _playerMovement.IsGrounded;

        #endregion Properties

        #region Constructors

        public ItemAction()
        {
            Cursor.Container.CollectionUpdated.AddListener(OnCursorCollecionUpdated);
            _player = Player.Instance;
            _playerMovement = _player.PlayerMovement;
        }

        ~ItemAction()
        {
            Cursor.Container.CollectionUpdated.RemoveListener(OnCursorCollecionUpdated);
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

        private void OnCursorCollecionUpdated()
        {
            OnMouseExit(null);
            OnLeftMouseButtonRelase();
            OnRightMouseButtonRelase();
        }

        #endregion Private
    }
}