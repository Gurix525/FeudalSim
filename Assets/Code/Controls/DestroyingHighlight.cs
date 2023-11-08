using Buildings;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class DestroyingHighlight : MonoBehaviour
    {
        #region Fields

        [SerializeField] private MainCursor _mainCursor;
        [SerializeField] private BuildingCursor _buildingCursor;
        [SerializeField] private Material _blockedMaterial;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Building _buildingPrefab;

        private bool _isRightMouseButtonPressed;
        private bool _isDestroying;
        private RaycastHit _cursorWorldHit;

        #endregion Fields

        #region Conditions

        private bool CanBeLeftClicked =>
            !_isRightMouseButtonPressed
            && !_mainCursor.IsOverUI
            && _isDestroying;

        #endregion Conditions

        #region Input

        private void OnLeftMouseButton(InputValue value)
        {
            if (!CanBeLeftClicked)
                return;
            if (value.isPressed)
            {
                DestroyBuilding();
                return;
            }
        }

        private void OnRightMouseButton(InputValue value)
        {
            _isRightMouseButtonPressed = value.isPressed;
        }

        #endregion Input

        #region Unity

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            _mainCursor.WorldPositionChanged += _mainCursor_WorldPositionChanged;
            _buildingCursor.DestroyingModeChanged += _buildingCursor_DestroyingModeChanged;
        }

        private void OnDisable()
        {
            _mainCursor.WorldPositionChanged -= _mainCursor_WorldPositionChanged;
            _buildingCursor.DestroyingModeChanged -= _buildingCursor_DestroyingModeChanged;
        }

        #endregion Unity

        #region Private

        private void DestroyBuilding()
        {
            if (_cursorWorldHit.collider.TryGetComponent(out Building building))
            {
                foreach (Item item in Item.GetFromRecipe(building.Recipe))
                {
                    InventoryCanvas.InventoryContainer.Insert(item);
                }
                Destroy(building.gameObject);
                _meshFilter.sharedMesh = null;
            }
        }

        private void _mainCursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            if (_isRightMouseButtonPressed)
                return;
            if (e.NewRaycastHit == null)
                return;
            _cursorWorldHit = e.NewRaycastHit.Value;
            if (_isDestroying)
            {
                if (_cursorWorldHit.collider.TryGetComponent(out Building building))
                {
                    var buildingMesh = building.GetComponent<MeshFilter>().sharedMesh;
                    _meshFilter.sharedMesh = buildingMesh;
                    transform.position = building.transform.position;
                    transform.rotation = building.transform.rotation;
                    transform.localScale = building.transform.localScale * 1.001F;
                }
                else
                    _meshFilter.sharedMesh = null;
            }
        }

        private void _buildingCursor_DestroyingModeChanged(object sender, bool e)
        {
            _isDestroying = e;
            if (!e)
                _meshFilter.sharedMesh = null;
        }

        #endregion Private
    }
}