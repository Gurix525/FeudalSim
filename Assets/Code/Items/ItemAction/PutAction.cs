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
        private bool _isControlMode = false;

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
            if (_isControlMode)
            {
                NoAction.OnLeftMouseButton();
                return;
            }
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
                            CursorItemMeshHighlight.Position)).transform)
                .GetComponent<ItemHandler>();
            itemHandler.Container.InsertAt(0, Cursor.Container.ExtractAt(0));
            itemHandler.transform.SetPositionAndRotation(
                CursorItemMeshHighlight.Position,
                CursorItemMeshHighlight.Rotation);
        }

        public override void OnRightMouseButton()
        {
            if (_isControlMode)
            {
                NoAction.OnRightMouseButton();
                return;
            }
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
                        CursorItemMeshHighlight.Position)).transform)
                .GetComponent<ItemHandler>();
            itemHandler.Container.InsertAt(0, Cursor.Container.ExtractAt(0, 1));
            itemHandler.transform.SetPositionAndRotation(
                CursorItemMeshHighlight.Position,
                CursorItemMeshHighlight.Rotation);
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
                PlayerController.MainControl.RemoveListener(ActionType.Started, EnableControlMode);
                PlayerController.MainControl.RemoveListener(ActionType.Canceled, DisableControlMode);
                CursorItemMeshHighlight.SetMesh(null);
                return;
            }
            PlayerController.MainChange.AddListener(ActionType.Started, ChangeRotation);
            PlayerController.MainControl.AddListener(ActionType.Started, EnableControlMode);
            PlayerController.MainControl.AddListener(ActionType.Canceled, DisableControlMode);
        }

        private void DisableControlMode(CallbackContext context)
        {
            _isControlMode = false;
        }

        private void EnableControlMode(CallbackContext context)
        {
            _isControlMode = true;
        }

        #endregion Private
    }
}