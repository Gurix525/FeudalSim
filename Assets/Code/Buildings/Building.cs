using AI;
using Controls;
using Extensions;
using Items;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        #region Fields

        private MeshRenderer _renderer;
        private MeshFilter _filter;
        private Item _backingItem;
        private BuildingMode _buildingMode;
        private Color _originalColor;
        private Color _previousColor;

        #endregion Fields

        #region Properties

        public Item BackingItem => _backingItem;

        public BuildingMode BuildingMode => _buildingMode;

        #endregion Properties

        #region Public

        public Item ExtractItem()
        {
            var output = _backingItem.Clone();
            _backingItem = null;
            return output;
        }

        public void AssignItem(Item item)
        {
            _backingItem = item;
        }

        public void DropItem()
        {
            if (_backingItem == null)
                return;
            _backingItem.Drop(transform.position);
            _backingItem = null;
        }

        public void Initialize(Item item, BuildingMode buildingMode)
        {
            _backingItem = item;
            _buildingMode = buildingMode;
            _filter = GetComponent<MeshFilter>();
            _filter.sharedMesh = _backingItem.BuildingMeshes[(int)_buildingMode];
            _renderer = GetComponent<MeshRenderer>();
            _renderer.sharedMaterial = _backingItem.Material;
            _renderer.material.SetFloat("_Displacement", buildingMode switch
            {
                BuildingMode.Floor => 0F,
                BuildingMode.BigFloor => 0F,
                BuildingMode.ShortWall => 1F,
                BuildingMode.Wall => 1F,
                BuildingMode.BigWall => 1F,
                _ => 0F,
            });
            _originalColor = _renderer.material.color;
            _previousColor = _originalColor;
        }

        public void ChangeColor(Color color)
        {
            if (_previousColor != color)
            {
                _renderer.material.color = color;
                _previousColor = color;
            }
        }

        public void ResetColor()
        {
            if (_previousColor != _originalColor)
            {
                _renderer.material.color = _originalColor;
                _previousColor = _originalColor;
            }
        }

        #endregion Public

        #region Unity

        //private void OnMouseEnter()
        //{
        //    Cursor.Action.OnMouseEnter(this);
        //    Cursor.Container.CollectionUpdated.AddListener(ResetItemAction);
        //}

        //private void OnMouseOver()
        //{
        //    Cursor.Action.OnMouseOver(this);
        //}

        //private void OnMouseExit()
        //{
        //    Cursor.Action.OnMouseExit(this);
        //    Cursor.Container.CollectionUpdated.RemoveListener(ResetItemAction);
        //}

        #endregion Unity

        #region Private

        private void ResetItemAction()
        {
            OnMouseExit();
            OnMouseEnter();
        }

        private void OnDestroy()
        {
            DropItem();
            Terrain.SetBuildingMark(
                transform.position.RoundToVector3Int(),
                _buildingMode,
                transform.rotation.y,
                false);
        }

        #endregion Private
    }
}