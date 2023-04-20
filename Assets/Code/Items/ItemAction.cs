﻿using System;
using Controls;
using Misc;
using UnityEngine;
using Cursor = Controls.Cursor;

namespace Items
{
    public abstract class ItemAction
    {
        public static NoAction NoAction { get; } = new();

        public Sprite Sprite => GetSprite();

        public ItemAction()
        {
            Cursor.Container.CollectionUpdated.AddListener(OnCursorCollecionUpdated);
        }

        ~ItemAction()
        {
            Cursor.Container.CollectionUpdated.RemoveListener(OnCursorCollecionUpdated);
        }

        public virtual void OnLeftMouseButton()
        { }

        public virtual void OnRightMouseButton()
        { }

        public virtual void Update()
        { }

        public virtual void OnMouseEnter(Component component)
        { }

        public virtual void OnMouseOver(Component component)
        { }

        public virtual void OnMouseExit(Component component)
        {
            (component as INoActionOutline)?.DisableOutline();
        }

        protected abstract Sprite GetSprite();

        private void OnCursorCollecionUpdated()
        {
            OnMouseExit(null);
        }
    }
}