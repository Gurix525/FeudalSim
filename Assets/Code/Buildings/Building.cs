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
        private MeshRenderer _renderer;
        private Item _backingItem;
        private BuildingMode _buildingMode;
        private Color _originalColor;
        private Color _previousColor;

        public void Initialize(Item item, BuildingMode buildingMode)
        {
            _backingItem = item;
            _buildingMode = buildingMode;
            _renderer = GetComponent<MeshRenderer>();
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

        private void OnMouseEnter()
        {
            if (Cursor.Item != null)
                Cursor.Item.Action.OnMouseEnter(this);
            Cursor.Container.CollectionUpdated.AddListener(ResetItemAction);
        }

        private void OnMouseOver()
        {
            if (Cursor.Item != null)
                Cursor.Item.Action.OnMouseOver(this);
        }

        private void OnMouseExit()
        {
            if (Cursor.Item != null)
                Cursor.Item.Action.OnMouseExit(this);
            Cursor.Container.CollectionUpdated.RemoveListener(ResetItemAction);
        }

        private void ResetItemAction()
        {
            OnMouseExit();
            OnMouseEnter();
        }

        private void OnDestroy()
        {
            Terrain.SetBuildingMark(
                transform.position.ToVector3Int(),
                _buildingMode,
                transform.rotation.y,
                false);
        }
    }
}