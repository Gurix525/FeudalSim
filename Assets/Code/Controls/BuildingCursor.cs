using System;
using Buildings;
using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class BuildingCursor : MonoBehaviour
    {
        #region Events

        public event EventHandler<ObjectChangedEventArgs> BuildingPrefabChanged;

        #endregion Events

        #region Fields

        [SerializeField] private MainCursor _mainCursor;
        //[SerializeField] private GameObject _meshHighlight;

        private GameObject _buildingPrefab;
        private Vector3 _pivotOffset;
        private Vector3 _cursorWorldPosition;
        private Vector3 _adjustedWorldPosition;
        private bool _isOverUI;

        #endregion Fields

        #region Properties

        public GameObject BuildingPrefab
        {
            get => _buildingPrefab;
            set
            {
                if (value != _buildingPrefab)
                {
                    ObjectChangedEventArgs eventArgs = new(_buildingPrefab, value);
                    _buildingPrefab = value;
                    BuildingPrefabChanged?.Invoke(this, eventArgs);
                }
            }
        }

        public static BuildingCursor Current { get; private set; }

        #endregion Properties

        #region Input

        private void OnLeftMouseButton(InputValue value)
        {
            if (value.isPressed)
                OnLeftMouseButtonPress();
            else
                OnLeftMouseButtonRelase();
        }

        #endregion Input

        #region Unity

        private void Awake()
        {
            Current = this;
            _mainCursor.WorldPositionChanged += _mainCursor_WorldPositionChanged;
            _mainCursor.OverUIStateChanged += _mainCursor_OverUIStateChanged;
            BuildingPrefabChanged += BuildingCursor_BuildingPrefabChanged;
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            BuildingPrefab = null;
        }

        #endregion Unity

        #region Private

        private void _mainCursor_OverUIStateChanged(object sender, bool e)
        {
            _isOverUI = e;
            if (_isOverUI)
                OnLeftMouseButtonRelase();
        }

        private void _mainCursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            //_cursorWorldPosition = e.NewRaycastHit.Value.point;
            //_adjustedWorldPosition = _cursorWorldPosition.Floor() + _pivotOffset;
            //_meshHighlight.transform.position = _adjustedWorldPosition;
        }

        private void BuildingCursor_BuildingPrefabChanged(object sender, ObjectChangedEventArgs e)
        {
            //if (e.NewObject != null)
            //{
            //    _pivotOffset = e.NewObject.GetComponent<Building>().PivotOffset;
            //    _meshHighlight.GetComponent<MeshFilter>().sharedMesh = e.NewObject.GetComponent<MeshFilter>().sharedMesh;
            //}
            //else
            //    _meshHighlight.GetComponent<MeshFilter>().sharedMesh = null;
        }

        private void OnLeftMouseButtonPress()
        {
        }

        private void OnLeftMouseButtonRelase()
        {
        }

        #endregion Private
    }
}