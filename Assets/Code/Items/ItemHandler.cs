using System;
using AI;
using Controls;
using Misc;
using UnityEngine;
using World;
using Cursor = Controls.PlayerCursor;
using Task = System.Threading.Tasks.Task;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ItemHandler : MonoBehaviour, IMouseHandler, IDetectable
    {
        #region Fields

        private bool _isClicked = false;

        #endregion

        #region Properties

        public Container Container { get; } = new(1);

        public Item Item => Container[0];

        #endregion Properties

        #region Public

        public async Task ScatterItem()
        {
            var meshCollider = GetComponent<MeshCollider>();
            meshCollider.convex = true;
            var rigidbody = gameObject.AddComponent<Rigidbody>();
            while (!rigidbody.IsSleeping())
            {
                await Task.Yield();
            }
            rigidbody.isKinematic = true;
            Destroy(rigidbody);
            meshCollider.convex = false;
            TerrainRenderer.MarkNavMeshToReload();
        }

        public void OnLeftMouseButton(Vector2 position)
        {
            _isClicked = true;
        }

        public void OnMousePosition(Vector2 position)
        {
            if (Item == null)
                return;
            _isClicked = false;
            Cursor.Current.ItemReference = new(Container, 0);
        }

        public void OnLeftMouseButtonRelase()
        {
            if (!_isClicked)
                return;
            _isClicked = false;
            Cursor.Current.RelaseItemReference();
            Item item = Container.ExtractAt(0);
            Equipment.InventoryContainer.Insert(item);
        }

        public void OnHoverEnd()
        {
            _isClicked = false;
        }

        //public void OnLeftMouseButton(Vector2 position)
        //{
        //    if (Item == null)
        //        return;
        //    if (Cursor.Item == null)
        //    {
        //        Cursor.Container.InsertAt(0, Container.ExtractAt(0));
        //        Destroy(gameObject);
        //        return;
        //    }
        //    if (Cursor.Item.Name == Item.Name)
        //    {
        //        Cursor.Container.InsertAt(0, Container.ExtractAt(0));
        //    }
        //    if (Cursor.IsNoActionActive)
        //        Equipment.Insert(Item);
        //    if (Item != null)
        //        if (Item.Count == 0)
        //            Container[0] = null;
        //    if (Item == null)
        //        Destroy(gameObject);
        //    TerrainRenderer.MarkNavMeshToReload();
        //}

        //public void OnRightMouseButton()
        //{
        //    if (Item == null)
        //        return;
        //    if (Cursor.Item == null)
        //    {
        //        Cursor.Container.InsertAt(0, Container.ExtractAt(0, 1));
        //        if (Item == null)
        //            Destroy(gameObject);
        //        return;
        //    }
        //    if (Cursor.Item.Name == Item.Name)
        //    {
        //        if (Cursor.Item.Count < Item.MaxStack)
        //        {
        //            Cursor.Container.InsertAt(0, Container.ExtractAt(0, 1));
        //        }
        //        else
        //        {
        //            var item = Container.ExtractAt(0, 1);
        //            Equipment.Insert(item);
        //            if (item.Count > 0)
        //                Container.InsertAt(0, item);
        //        }
        //        if (Item == null)
        //            Destroy(gameObject);
        //        return;
        //    }
        //    if (Cursor.IsNoActionActive)
        //    {
        //        var item = Container.ExtractAt(0, 1);
        //        if (item == null)
        //            return;
        //        Equipment.Insert(item);
        //        if (item.Count > 0)
        //            Container.InsertAt(0, item);
        //        if (Item != null)
        //            if (Item.Count == 0)
        //                Container[0] = null;
        //        if (Item == null)
        //            Destroy(gameObject);
        //    }
        //    TerrainRenderer.MarkNavMeshToReload();
        //}

        #endregion Public

        #region Unity

        private void OnEnable()
        {
            Container.CollectionUpdated.AddListener(OnCollectionUpdated);
        }

        private void OnDisable()
        {
            Container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
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