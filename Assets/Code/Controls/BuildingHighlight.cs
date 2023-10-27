using Buildings;
using Extensions;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using World;

namespace Controls
{
    public class BuildingHighlight : MonoBehaviour
    {
        #region Fields

        [SerializeField] private MainCursor _mainCursor;
        [SerializeField] private BuildingCursor _buildingCursor;
        [SerializeField] private Material _notBlockedMaterial;

        private MeshFilter _meshFilter;
        private PlayerInput _playerInput;
        private MeshRenderer _meshRenderer;
        private Building _buildingPrefab;

        private bool _isRightMouseButtonPressed;
        private bool _isPlacing;
        private RaycastHit _cursorWorldHit;
        private Vector3 _placingStartPosition;

        #endregion Fields

        #region Conditions

        private bool CanBeLeftClicked =>
            !_isRightMouseButtonPressed
            && _buildingPrefab != null
            && !_mainCursor.IsOverUI;

        #endregion Conditions

        #region Input

        private void OnLeftMouseButton(InputValue value)
        {
            if (!CanBeLeftClicked)
                return;
            if (value.isPressed)
            {
                //StartPlacing();
                PlaceBuilding();
                return;
            }
            //EndPlacing();
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
            _playerInput = GetComponent<PlayerInput>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            _buildingCursor.BuildingPrefabChanged += _buildingCursor_BuildingPrefabChanged;
            _mainCursor.WorldPositionChanged += _mainCursor_WorldPositionChanged;
        }

        private void OnDisable()
        {
            _buildingCursor.BuildingPrefabChanged -= _buildingCursor_BuildingPrefabChanged;
            _mainCursor.WorldPositionChanged -= _mainCursor_WorldPositionChanged;
            _isPlacing = false;
            EndPlacing();
        }

        #endregion Unity

        #region Private

        private void StartPlacing()
        {
            _placingStartPosition = _cursorWorldHit.point;
            _isPlacing = true;
        }

        private void EndPlacing()
        {
            if (!_isPlacing)
                return;

            _isPlacing = false;
        }

        private void PlaceBuilding()
        {
            if (!Equipment.InventoryContainer.MatchesRecipe(_buildingPrefab.Recipe))
                return;
            Equipment.InventoryContainer.RemoveRecipeItems(_buildingPrefab.Recipe);
            Vector2Int chunkPosition = World.Terrain.GetChunkCoordinates(
                new Vector2(transform.position.x, transform.position.z));
            ChunkRenderer chunkRenderer = TerrainRenderer.GetChunkRenderer(chunkPosition);
            Instantiate(_buildingPrefab, transform.position, transform.rotation, chunkRenderer.Buildings);
        }

        private void PlaceGrid()
        {
            // To be added
            Plane plane = new(Vector3.up, _placingStartPosition.y.Round());
            Ray ray = Camera.main.ScreenPointToRay(_mainCursor.ScreenPosition);
            plane.Raycast(ray, out float distance);
            Vector3 planeHit = ray.origin + ray.direction * distance;
            var start = _placingStartPosition.Floor();
            var end = planeHit.Floor();
        }

        private void _mainCursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            if (_isRightMouseButtonPressed)
                return;
            if (e.NewRaycastHit == null)
                return;
            if (_buildingPrefab == null)
                return;
            _cursorWorldHit = e.NewRaycastHit.Value;
            transform.position = (_cursorWorldHit.point + Vector3.up * 0.001F).Floor() + _buildingPrefab.PivotOffset;
            if (_isPlacing)
            {
                //PlaceGrid();
            }
        }

        private void _buildingCursor_BuildingPrefabChanged(object sender, ObjectChangedEventArgs e)
        {
            _isPlacing = false;
            EndPlacing();
            if (e.NewObject == null)
            {
                _buildingPrefab = null;
                _meshFilter.sharedMesh = null;
                return;
            }
            _buildingPrefab = e.NewObject.GetComponent<Building>();
            _meshFilter.sharedMesh = _buildingPrefab.GetComponent<MeshFilter>().sharedMesh;
        }

        #endregion Private
    }
}