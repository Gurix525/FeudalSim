using Buildings;
using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class BuildingHighlight : MonoBehaviour
    {
        [SerializeField] private MainCursor _mainCursor;
        [SerializeField] private BuildingCursor _buildingCursor;

        private MeshFilter _meshFilter;
        private PlayerInput _playerInput;
        private MeshRenderer _meshRenderer;
        private Building _buildingPrefab;

        private bool _isRightMouseButtonPressed;

        private void OnLeftMouseButton(InputValue value)
        {
            if (_isRightMouseButtonPressed || _buildingPrefab == null)
                return;
            Instantiate(_buildingPrefab, transform.position, Quaternion.identity);
        }

        private void OnRightMouseButton(InputValue value)
        {
            _isRightMouseButtonPressed = value.isPressed;
        }

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
        }

        private void _mainCursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            if (_isRightMouseButtonPressed)
                return;
            if (e.NewRaycastHit == null)
                return;
            if (_buildingPrefab == null)
                return;
            transform.position = e.NewRaycastHit.Value.point.Floor() + _buildingPrefab.PivotOffset;
        }

        private void _buildingCursor_BuildingPrefabChanged(object sender, ObjectChangedEventArgs e)
        {
            if (e.NewObject == null)
            {
                _buildingPrefab = null;
                _meshFilter.sharedMesh = null;
                return;
            }
            _buildingPrefab = e.NewObject.GetComponent<Building>();
            _meshFilter.sharedMesh = _buildingPrefab.GetComponent<MeshFilter>().sharedMesh;
        }
    }
}