using Misc;
using Cursor = Controls.Cursor;
using UnityEngine;
using Controls;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ItemHandler : MonoBehaviour, IClickable
    {
        #region Fields

        private OutlineHandler _outlineHandler;

        #endregion Fields

        #region Properties

        public Container Container = new(1);

        public Item Item => Container[0];

        #endregion Properties

        #region Public

        public void Click()
        {
            if (Cursor.Item == null)
            {
                Cursor.Container.InsertAt(0, Container.ExtractAt(0));
                Destroy(gameObject);
                return;
            }
            if (Cursor.Item.Name == Item.Name)
            {
                int delta = Mathf.Min(Item.Count, Item.MaxStack - Cursor.Item.Count);
                Container.InsertAt(0, Container.ExtractAt(0, delta));
                if (Item == null)
                    Destroy(gameObject);
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
            Container.CollectionUpdated.AddListener(OnCollectionUpdated);
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

        #endregion Private
    }
}