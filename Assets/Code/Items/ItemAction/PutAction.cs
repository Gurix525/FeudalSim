using System;
using Controls;
using Input;
using Misc;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;

namespace Items
{
    public class PutAction : ItemAction
    {
        #region Fields

        private float _meshRotation;

        #endregion Fields

        #region Constructors

        public PutAction()
        {
            Cursor.Container.CollectionUpdated.AddListener(OnCursorCollectionUpdated);
        }

        #endregion Constructors

        #region Public

        public override void OnLeftMouseButton()
        {
            if (Cursor.CurrentRaycastHit == null)
                return;
            if (CursorItemMeshHighlight.IsBlocked)
                return;
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Items/" + Cursor.Item.Name);
            if (prefab == null)
                return;
            ItemHandler itemHandler = GameObject
                .Instantiate(prefab, TerrainRenderer.GetChunkRenderer(
                        Terrain.GetChunkCoordinates(
                            CursorItemMeshHighlight.Position)).ItemHandlers)
                .GetComponent<ItemHandler>();
            Item inputItem = Cursor.Container.ExtractAt(0, 0);
            if (inputItem == null)
            {
                inputItem = Cursor.Item.Clone(0);
                Cursor.Item.Count = 0;
                Equipment.ClearEmptyItems();
            }
            itemHandler.Container.InsertAt(0, inputItem);
            itemHandler.transform.SetPositionAndRotation(
                CursorItemMeshHighlight.Position,
                CursorItemMeshHighlight.Rotation);
            TerrainRenderer.MarkNavMeshToReload();
        }

        public override void OnRightMouseButton()
        {
            if (Cursor.CurrentRaycastHit == null)
                return;
            if (CursorItemMeshHighlight.IsBlocked)
                return;
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Items/" + Cursor.Item.Name);
            if (prefab == null)
                return;
            ItemHandler itemHandler = GameObject
                .Instantiate(prefab, TerrainRenderer.GetChunkRenderer(
                    Terrain.GetChunkCoordinates(
                        CursorItemMeshHighlight.Position)).ItemHandlers)
                .GetComponent<ItemHandler>();
            Item inputItem = Cursor.Container.ExtractAt(0, 1);
            if (inputItem == null)
            {
                inputItem = Cursor.Item.Clone(1);
                Cursor.Item.Count--;
                Equipment.ClearEmptyItems();
            }
            itemHandler.Container.InsertAt(0, inputItem);
            itemHandler.transform.SetPositionAndRotation(
                CursorItemMeshHighlight.Position,
                CursorItemMeshHighlight.Rotation);
            TerrainRenderer.MarkNavMeshToReload();
        }

        public override void Update()
        {
            CursorItemMeshHighlight.SetMesh(Cursor.Item.Mesh);
            CursorItemMeshHighlight.SetMeshRotation(_meshRotation);
        }

        #endregion Public

        #region Protected

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/PutAction");
        }

        #endregion Protected

        #region Private

        private void ChangeRotation(CallbackContext context)
        {
            _meshRotation += _meshRotation == -270F ? 270F : -90F;
        }

        private void OnCursorCollectionUpdated()
        {
            if (Cursor.Action != this)
            {
                PlayerController.MainChange.RemoveListener(ActionType.Started, ChangeRotation);
                CursorItemMeshHighlight.SetMesh(null);
                return;
            }
            PlayerController.MainChange.AddListener(ActionType.Started, ChangeRotation);
        }

        #endregion Private
    }
}