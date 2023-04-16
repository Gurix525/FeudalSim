using Misc;
using Cursor = Controls.Cursor;
using UnityEngine;
using Controls;
using Input;
using UnityEngine.InputSystem;
using System;
using static UnityEngine.InputSystem.InputAction;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ItemHandler : MonoBehaviour, ILeftClickHandler, IRightClickHandler
    {
        #region Fields

        private OutlineHandler _outlineHandler;
        private bool _isStackMode = false;

        #endregion Fields

        #region Properties

        public Container Container = new(1);

        public Item Item => Container[0];

        #endregion Properties

        #region Public

        public void OnLeftMouseButton()
        {
            if (Item == null)
                return;
            if (Cursor.Item == null)
            {
                Cursor.Container.InsertAt(0, Container.ExtractAt(0));
                Destroy(gameObject);
                return;
            }
            if (Cursor.Item.Name == Item.Name)
            {
                int delta = Mathf.Min(Item.Count, Item.MaxStack - Cursor.Item.Count);
                if (delta == 0)
                    return;
                Cursor.Container.InsertAt(0, Container.ExtractAt(0, delta));
                if (Item == null)
                    Destroy(gameObject);
            }
        }

        public void OnRightMouseButton()
        {
            if (Item == null)
                return;
            if (Cursor.Item == null)
            {
                Cursor.Container.InsertAt(0, Container.ExtractAt(0, 1));
                if (Item == null)
                    Destroy(gameObject);
                return;
            }
            if (Cursor.Item.Name == Item.Name)
            {
                if (Cursor.Item.Count < Item.MaxStack)
                {
                    Cursor.Container.InsertAt(0, Container.ExtractAt(0, 1));
                    if (Item == null)
                        Destroy(gameObject);
                }
            }
        }

        public void EnableOutline()
        {
            _outlineHandler.EnableOutline();
        }

        public void DisableOutline()
        {
            _outlineHandler.DisableOutline();
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _outlineHandler = GetComponent<OutlineHandler>();
        }

        private void OnEnable()
        {
            Container.CollectionUpdated.AddListener(OnCollectionUpdated);
            PlayerController.MainControl.AddListener(ActionType.Started, EnableStackMode);
        }

        private void OnDisable()
        {
            Container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
            PlayerController.MainControl.RemoveListener(ActionType.Started, EnableStackMode);
        }

        private void OnMouseOver()
        {
            Cursor.Action.OnMouseOver(this);
        }

        private void OnMouseExit()
        {
            Cursor.Action.OnMouseExit(this);
        }

        #endregion Unity

        #region Private

        private void OnCollectionUpdated()
        {
            if (Container[0] == null)
                Destroy(gameObject);
        }

        private void EnableStackMode(CallbackContext context)
        {
            _isStackMode = true;
            PlayerController.MainControl.AddListener(ActionType.Canceled, DisableStackMode);
            PlayerController.MainControl.RemoveListener(ActionType.Started, EnableStackMode);
        }

        private void DisableStackMode(CallbackContext context)
        {
            _isStackMode = false;
            PlayerController.MainControl.RemoveListener(ActionType.Canceled, DisableStackMode);
            PlayerController.MainControl.AddListener(ActionType.Started, EnableStackMode);
        }

        #endregion Private
    }
}